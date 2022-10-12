namespace SplitAdmin
{
    public class HttpRetryHandler : DelegatingHandler
    {
        private const int MaxRetries = 10;
        private int remainingRequests = 10;
        private int newWindowTime = 0;

        public HttpRetryHandler(HttpMessageHandler innerHandler) : base(innerHandler) { }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage? response = null;

            for (int i = 0; i < MaxRetries; i++)
            {
                // The window resolution is 1 second, so we don't know if we are at the start
                // or end of it. To be on the safe side, we add a second to ensure that we
                // don't accidentally call in the same window again.
                var sleepTime = (newWindowTime + 1) * 1000;

                if (remainingRequests <= 1)
                {
                    Thread.Sleep(sleepTime);
                }

                response = await base.SendAsync(request, cancellationToken);

                try
                {
                    var remainingOrgString = response.Headers.GetValues("X-RateLimit-Remaining-Org").First();
                    var remainingIpString = response.Headers.GetValues("X-RateLimit-Remaining-IP").First();
                    var remainingSecondsOrgString = response.Headers.GetValues("X-RateLimit-Reset-Seconds-Org").First();
                    var remainingSecondsIpString = response.Headers.GetValues("X-RateLimit-Reset-Seconds-IP").First();

                    var remainingOrg = int.Parse(remainingOrgString);
                    var remainingIp = int.Parse(remainingIpString);
                    var remainingSecondsOrg = int.Parse(remainingSecondsOrgString);
                    var remainingSecondsIp = int.Parse(remainingSecondsIpString);

                    remainingRequests = Math.Min(remainingOrg, remainingIp);
                    newWindowTime = Math.Max(remainingSecondsIp, remainingSecondsOrg);
                }
                catch (InvalidOperationException)
                {
                    // Parse error. Just ignore
                }

                // We shouldn't ever hit this, but just in case
                if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    Thread.Sleep(sleepTime);
                    continue;
                }

                // We continue on 429 above, but ignore everything else
                return response;
            }

            if (response == null)
            {
                // This should never be thrown as we always populate it inside the loop above
                throw new SplitAdminException("Response was null");
            }

            return response;
        }
    }
}
