﻿using Dapper;

namespace Movies.Application.Database;

public class DbInitializer(IDbConnectionFactory dbConnectionFactory)
{
    public async Task InitializeAsync()
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync();
        await connection.ExecuteAsync("""
                                      create table if not exists movies(
                                          id UUID primary key,
                                          slug TEXT not null,
                                          title TEXT not null,
                                          yearofrelease integer not null);
                                      """);
        await connection.ExecuteAsync("""
                                      create unique index concurrently if not exists movie_slug_idx
                                      on movies
                                      using btree(slug);
                                      """);
        await connection.ExecuteAsync("""
                                      create table if not exists genres(
                                          movieid UUID references movies (Id),
                                          name TEXT not null);
                                      """);
    }
}