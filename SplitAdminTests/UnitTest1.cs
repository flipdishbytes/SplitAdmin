using SplitAdmin;
using System.Reflection;

namespace SplitAdminTests
{
    [TestClass]
    public class UnitTest1
    {
        public UnitTest1()
        {
            var projectLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (projectLocation == null)
            {
                throw new FileNotFoundException("Failed to get project location");
            }

            string path = Path.Combine(projectLocation, ".env");

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Could not find .env file");
            }

            foreach (var line in File.ReadAllLines(path))
            {
                var components = line.Split('=', StringSplitOptions.RemoveEmptyEntries);

                if (components.Length != 2)
                {
                    throw new InvalidDataException("Invalid .env file");
                }

                Environment.SetEnvironmentVariable(components[0], components[1]);
            }
        }

        private static string GetKey()
        {
            var key = Environment.GetEnvironmentVariable("SPLIT_ADMIN_KEY");
            if (key == null)
            {
                throw new KeyNotFoundException("Failed to get key");
            }
            return key;
        }

        [TestMethod]
        public async Task TestGetWorkspaces()
        {
            var client = new SplitClient(GetKey());
            var workspaces = await client.Workspaces.GetAll();
            Assert.AreNotEqual(workspaces.Count, 0);
        }

        [TestMethod]
        public async Task TestGetEnvironments()
        {
            var client = new SplitClient(GetKey());
            var workspaces = await client.Workspaces.GetAll();
            foreach (var workspace in workspaces)
            {
                var environments = await client.Environments.Get(workspace);
                Assert.AreNotEqual(environments.Count, 0);
            }
        }

        [TestMethod]
        public async Task TestGetSplits()
        {
            var client = new SplitClient(GetKey());
            var workspaces = await client.Workspaces.GetAll();
            foreach (var workspace in workspaces)
            {
                var splits = await client.Splits.GetAll(workspace);
                Assert.AreNotEqual(splits.Count, 0);
            }
        }

        [TestMethod]
        public async Task TestGetSplitsInEnvironments()
        {
            var client = new SplitClient(GetKey());
            var workspaces = await client.Workspaces.GetAll();
            foreach (var workspace in workspaces)
            {
                var environments = await client.Environments.Get(workspace);
                foreach (var environment in environments)
                {
                    var splits = await client.Splits.GetAll(workspace, environment.Name);
                    Assert.AreNotEqual(splits.Count, 0);
                }
            }
        }

        [TestMethod]
        public async Task TestGetTrafficTypes()
        {
            var client = new SplitClient(GetKey());
            var workspaces = await client.Workspaces.GetAll();
            foreach (var workspace in workspaces)
            {
                var types = await client.TrafficTypes.Get(workspace);
                Assert.AreNotEqual(types.Count, 0);
            }
        }

        [TestMethod]
        public async Task TestGetAttributes()
        {
            var client = new SplitClient(GetKey());
            var workspaces = await client.Workspaces.GetAll();
            foreach (var workspace in workspaces)
            {
                var types = await client.TrafficTypes.Get(workspace);
                foreach (var trafficType in types)
                {
                    var attrs = await client.Attributes.Get(workspace, trafficType);
                    Assert.IsNotNull(attrs);
                }
            }
        }

        [TestMethod]
        public async Task TestGetUser()
        {
            var client = new SplitClient(GetKey());
            var user = await client.Users.Get("a93ef0a0-3d91-11ed-92a9-c2b444dae212");
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public async Task TestGetSegments()
        {
            var client = new SplitClient(GetKey());
            var workspaces = await client.Workspaces.GetAll();
            foreach (var workspace in workspaces)
            {
                var segments = await client.Segments.GetAll(workspace);
                Assert.AreNotEqual(segments.Count, 0);
            }
        }
    }
}