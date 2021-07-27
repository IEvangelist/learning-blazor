// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Learning.Blazor.Abstractions
{
    public interface ITwitterHubClient
    {
        public ValueTask JoinTwitterGroup();

        public ValueTask LeaveTwitterGroup();

        public ValueTask TweetReceived
    }
}
