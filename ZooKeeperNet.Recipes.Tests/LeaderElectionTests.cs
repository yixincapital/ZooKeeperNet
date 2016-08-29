/*
 *  Licensed to the Apache Software Foundation (ASF) under one or more
 *  contributor license agreements.  See the NOTICE file distributed with
 *  this work for additional information regarding copyright ownership.
 *  The ASF licenses this file to You under the Apache License, Version 2.0
 *  (the "License"); you may not use this file except in compliance with
 *  the License.  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 *
 */

using System;
using System.Threading;
using NUnit.Framework;
using ZooKeeperNet;
using ZooKeeperNet.Recipes;
using ZooKeeperNet.Tests;
using log4net;

namespace ZooKeeperNetRecipes.Tests {
	[TestFixture]
	public class LeaderElectionTests : AbstractZooKeeperTests {
		private static ILog LOG = LogManager.GetLogger(typeof (LeaderElectionTests));
		private ZooKeeper[] clients;

		[TearDown]
		public void Teardown() {
			foreach (var zk in clients)
				zk.Dispose();
		}

		private class TestLeaderWatcher : ILeaderWatcher {
			public byte Leader;
			private readonly byte b;

			public TestLeaderWatcher(byte b) {
				this.b = b;
			}

			public void TakeLeadership() {
				Leader = b;
				LOG.DebugFormat("Leader: {0:x}", b);
			}
		}

		[Test]
		public void testElection() {
			String dir = "/test";
			int num_clients = 10;
			clients = new ZooKeeper[num_clients];
			LeaderElection[] elections = new LeaderElection[num_clients];
			for (byte i = 0; i < clients.Length; i++) {
				clients[i] = CreateClient();
				elections[i] = new LeaderElection(clients[i], dir, new TestLeaderWatcher(i), new[] {i});
				elections[i].Start();
			}

			for (byte i = 0; i < clients.Length; i++) {
				while (!elections[i].IsOwner) {
					Thread.Sleep(1);
				}
				elections[i].Close();
			}
			Assert.Pass();
		}

		[Test]
		public void testNode4DoesNotBecomeLeaderWhenNonLeader3Closes()
		{
			var dir = "/test";
			var num_clients = 4;
			clients = new ZooKeeper[num_clients];
			var elections = new LeaderElection[num_clients];
			var leaderWatchers = new TestLeaderWatcher[num_clients];

			for (byte i = 0; i < clients.Length; i++)
			{
				clients[i] = CreateClient();
				leaderWatchers[i] = new TestLeaderWatcher((byte)(i + 1)); // Start at 1 so we can check it is set
				elections[i] = new LeaderElection(clients[i], dir, leaderWatchers[i], new[] { i });
				elections[i].Start();
			}

			// Kill 2
			elections[2].Close();
			// First one should still be leader
			Thread.Sleep(3000);
			Assert.AreEqual(1, leaderWatchers[0].Leader);
			Assert.AreEqual(0, leaderWatchers[1].Leader);
			Assert.AreEqual(0, leaderWatchers[2].Leader);
			Assert.AreEqual(0, leaderWatchers[3].Leader);
			elections[0].Close();
			elections[1].Close();
			elections[3].Close();
		}
	}
}

