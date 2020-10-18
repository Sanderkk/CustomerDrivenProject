﻿using System;
using System.Text.Json;
using HotChocolate;
using HotChocolate.Types;
using src.Api.Inputs;
using src.Database;
using src.Database.User;

namespace src.Api.Mutations
{
    [ExtendObjectType(Name = "Mutation")]
    public class DashboardMutation
    {
        

        public bool UpdateDashboard(
            DashboardInput input,
            [Service] IUserRepository repo
        )
        {
            JsonElement jsonData = JsonSerializer.Deserialize<JsonElement>(input.data);
            string queryString = UserQueryBuilder.InsertOrUpdateDashboardQueryString(input.name, input.description, jsonData);
            return repo.UpdateDashboard(input.userId, input.accessLevel, queryString).Result;
        }
        
        public bool CreateDashboard(
            [GraphQLNonNullType] string userId,
            string accessLevel,
            string name,
            string description,
            [GraphQLNonNullType] string data,
            [Service] IUserRepository repo
        )
        {
            JsonElement jsonData = JsonSerializer.Deserialize<JsonElement>(data);
            string queryString = UserQueryBuilder.CreateDashboardQueryString(name, description, jsonData);
            return repo.CreateDashboard(queryString).Result;
        }
    }
}