﻿using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SoundCloud.Api.Entities;

namespace SoundCloud.Api.IntegrationTest
{
    [TestFixture]
    public class CommentsTest : IntegrationTestBase
    {
        [Test]
        public async Task Test_Comment_Get()
        {
            var client = SoundCloudClient.CreateUnauthorized(Settings.ClientId);

            var result = await client.Comments.GetAsync(256985338);

            Assert.That(result.Body, Does.Contain("TestComment"));
        }

        [Test]
        public async Task Test_Comment_GetList()
        {
            var client = SoundCloudClient.CreateUnauthorized(Settings.ClientId);

            var result = await client.Comments.GetAllAsync();
            Assert.That(result.Any(), Is.True);

            if (result.HasNextPage)
            {
                var nextResult = await result.GetNextPageAsync();
                Assert.That(nextResult.Any(), Is.True);

                Assert.That(result.First().Id, Is.Not.EqualTo(nextResult.First().Id));
            }
        }

        [Test]
        public async Task Test_Comment_Post_Delete()
        {
            var client = SoundCloudClient.CreateAuthorized(Settings.Token);

            var comment = new Comment { Body = "TestComment at " + DateTime.Now.ToLocalTime(), TrackId = TrackId };

            var postResult = await client.Comments.PostAsync(comment);
            Assert.That(postResult.Body, Is.EqualTo(comment.Body));

            var deleteResult = await client.Comments.DeleteAsync(postResult);
            Assert.That(deleteResult.Errors, Is.Empty);
        }
    }
}
