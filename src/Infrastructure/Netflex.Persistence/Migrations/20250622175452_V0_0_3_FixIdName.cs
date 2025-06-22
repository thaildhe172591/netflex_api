using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Netflex.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class V0_0_3_FixIdName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_episode_actors_actors_actors_id",
                schema: "dbo",
                table: "episode_actors");

            migrationBuilder.DropForeignKey(
                name: "fk_movie_actors_actors_actors_id",
                schema: "dbo",
                table: "movie_actors");

            migrationBuilder.DropForeignKey(
                name: "fk_movie_genres_genres_genres_id",
                schema: "dbo",
                table: "movie_genres");

            migrationBuilder.DropForeignKey(
                name: "fk_movie_keywords_keywords_keywords_id",
                schema: "dbo",
                table: "movie_keywords");

            migrationBuilder.DropForeignKey(
                name: "fk_role_permissions_permissions_permissions_id",
                schema: "dbo",
                table: "role_permissions");

            migrationBuilder.DropForeignKey(
                name: "fk_role_permissions_roles_roles_id",
                schema: "dbo",
                table: "role_permissions");

            migrationBuilder.DropForeignKey(
                name: "fk_tv_serie_genres_genres_genres_id",
                schema: "dbo",
                table: "tv_serie_genres");

            migrationBuilder.DropForeignKey(
                name: "fk_tv_serie_keywords_keywords_keywords_id",
                schema: "dbo",
                table: "tv_serie_keywords");

            migrationBuilder.DropForeignKey(
                name: "fk_user_permissions_permissions_permissions_id",
                schema: "dbo",
                table: "user_permissions");

            migrationBuilder.DropForeignKey(
                name: "fk_user_permissions_users_users_id",
                schema: "dbo",
                table: "user_permissions");

            migrationBuilder.DropForeignKey(
                name: "fk_user_roles_roles_roles_id",
                schema: "dbo",
                table: "user_roles");

            migrationBuilder.DropForeignKey(
                name: "fk_user_roles_users_users_id",
                schema: "dbo",
                table: "user_roles");

            migrationBuilder.RenameColumn(
                name: "users_id",
                schema: "dbo",
                table: "user_roles",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "roles_id",
                schema: "dbo",
                table: "user_roles",
                newName: "role_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_roles_users_id",
                schema: "dbo",
                table: "user_roles",
                newName: "ix_user_roles_user_id");

            migrationBuilder.RenameColumn(
                name: "users_id",
                schema: "dbo",
                table: "user_permissions",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "permissions_id",
                schema: "dbo",
                table: "user_permissions",
                newName: "permission_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_permissions_users_id",
                schema: "dbo",
                table: "user_permissions",
                newName: "ix_user_permissions_user_id");

            migrationBuilder.RenameColumn(
                name: "keywords_id",
                schema: "dbo",
                table: "tv_serie_keywords",
                newName: "keyword_id");

            migrationBuilder.RenameColumn(
                name: "genres_id",
                schema: "dbo",
                table: "tv_serie_genres",
                newName: "genre_id");

            migrationBuilder.RenameColumn(
                name: "roles_id",
                schema: "dbo",
                table: "role_permissions",
                newName: "role_id");

            migrationBuilder.RenameColumn(
                name: "permissions_id",
                schema: "dbo",
                table: "role_permissions",
                newName: "permission_id");

            migrationBuilder.RenameIndex(
                name: "ix_role_permissions_roles_id",
                schema: "dbo",
                table: "role_permissions",
                newName: "ix_role_permissions_role_id");

            migrationBuilder.RenameColumn(
                name: "keywords_id",
                schema: "dbo",
                table: "movie_keywords",
                newName: "keyword_id");

            migrationBuilder.RenameColumn(
                name: "genres_id",
                schema: "dbo",
                table: "movie_genres",
                newName: "genre_id");

            migrationBuilder.RenameColumn(
                name: "actors_id",
                schema: "dbo",
                table: "movie_actors",
                newName: "actor_id");

            migrationBuilder.RenameColumn(
                name: "actors_id",
                schema: "dbo",
                table: "episode_actors",
                newName: "actor_id");

            migrationBuilder.AddForeignKey(
                name: "fk_episode_actors_actors_actor_id",
                schema: "dbo",
                table: "episode_actors",
                column: "actor_id",
                principalSchema: "dbo",
                principalTable: "actors",
                principalColumn: "actor_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_movie_actors_actors_actor_id",
                schema: "dbo",
                table: "movie_actors",
                column: "actor_id",
                principalSchema: "dbo",
                principalTable: "actors",
                principalColumn: "actor_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_movie_genres_genres_genre_id",
                schema: "dbo",
                table: "movie_genres",
                column: "genre_id",
                principalSchema: "dbo",
                principalTable: "genres",
                principalColumn: "genre_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_movie_keywords_keywords_keyword_id",
                schema: "dbo",
                table: "movie_keywords",
                column: "keyword_id",
                principalSchema: "dbo",
                principalTable: "keywords",
                principalColumn: "keyword_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_role_permissions_permissions_permission_id",
                schema: "dbo",
                table: "role_permissions",
                column: "permission_id",
                principalSchema: "dbo",
                principalTable: "permissions",
                principalColumn: "permission_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_role_permissions_roles_role_id",
                schema: "dbo",
                table: "role_permissions",
                column: "role_id",
                principalSchema: "dbo",
                principalTable: "roles",
                principalColumn: "role_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_tv_serie_genres_genres_genre_id",
                schema: "dbo",
                table: "tv_serie_genres",
                column: "genre_id",
                principalSchema: "dbo",
                principalTable: "genres",
                principalColumn: "genre_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_tv_serie_keywords_keywords_keyword_id",
                schema: "dbo",
                table: "tv_serie_keywords",
                column: "keyword_id",
                principalSchema: "dbo",
                principalTable: "keywords",
                principalColumn: "keyword_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_permissions_permissions_permission_id",
                schema: "dbo",
                table: "user_permissions",
                column: "permission_id",
                principalSchema: "dbo",
                principalTable: "permissions",
                principalColumn: "permission_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_permissions_users_user_id",
                schema: "dbo",
                table: "user_permissions",
                column: "user_id",
                principalSchema: "dbo",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_roles_roles_role_id",
                schema: "dbo",
                table: "user_roles",
                column: "role_id",
                principalSchema: "dbo",
                principalTable: "roles",
                principalColumn: "role_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_roles_users_user_id",
                schema: "dbo",
                table: "user_roles",
                column: "user_id",
                principalSchema: "dbo",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_episode_actors_actors_actor_id",
                schema: "dbo",
                table: "episode_actors");

            migrationBuilder.DropForeignKey(
                name: "fk_movie_actors_actors_actor_id",
                schema: "dbo",
                table: "movie_actors");

            migrationBuilder.DropForeignKey(
                name: "fk_movie_genres_genres_genre_id",
                schema: "dbo",
                table: "movie_genres");

            migrationBuilder.DropForeignKey(
                name: "fk_movie_keywords_keywords_keyword_id",
                schema: "dbo",
                table: "movie_keywords");

            migrationBuilder.DropForeignKey(
                name: "fk_role_permissions_permissions_permission_id",
                schema: "dbo",
                table: "role_permissions");

            migrationBuilder.DropForeignKey(
                name: "fk_role_permissions_roles_role_id",
                schema: "dbo",
                table: "role_permissions");

            migrationBuilder.DropForeignKey(
                name: "fk_tv_serie_genres_genres_genre_id",
                schema: "dbo",
                table: "tv_serie_genres");

            migrationBuilder.DropForeignKey(
                name: "fk_tv_serie_keywords_keywords_keyword_id",
                schema: "dbo",
                table: "tv_serie_keywords");

            migrationBuilder.DropForeignKey(
                name: "fk_user_permissions_permissions_permission_id",
                schema: "dbo",
                table: "user_permissions");

            migrationBuilder.DropForeignKey(
                name: "fk_user_permissions_users_user_id",
                schema: "dbo",
                table: "user_permissions");

            migrationBuilder.DropForeignKey(
                name: "fk_user_roles_roles_role_id",
                schema: "dbo",
                table: "user_roles");

            migrationBuilder.DropForeignKey(
                name: "fk_user_roles_users_user_id",
                schema: "dbo",
                table: "user_roles");

            migrationBuilder.RenameColumn(
                name: "user_id",
                schema: "dbo",
                table: "user_roles",
                newName: "users_id");

            migrationBuilder.RenameColumn(
                name: "role_id",
                schema: "dbo",
                table: "user_roles",
                newName: "roles_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_roles_user_id",
                schema: "dbo",
                table: "user_roles",
                newName: "ix_user_roles_users_id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                schema: "dbo",
                table: "user_permissions",
                newName: "users_id");

            migrationBuilder.RenameColumn(
                name: "permission_id",
                schema: "dbo",
                table: "user_permissions",
                newName: "permissions_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_permissions_user_id",
                schema: "dbo",
                table: "user_permissions",
                newName: "ix_user_permissions_users_id");

            migrationBuilder.RenameColumn(
                name: "keyword_id",
                schema: "dbo",
                table: "tv_serie_keywords",
                newName: "keywords_id");

            migrationBuilder.RenameColumn(
                name: "genre_id",
                schema: "dbo",
                table: "tv_serie_genres",
                newName: "genres_id");

            migrationBuilder.RenameColumn(
                name: "role_id",
                schema: "dbo",
                table: "role_permissions",
                newName: "roles_id");

            migrationBuilder.RenameColumn(
                name: "permission_id",
                schema: "dbo",
                table: "role_permissions",
                newName: "permissions_id");

            migrationBuilder.RenameIndex(
                name: "ix_role_permissions_role_id",
                schema: "dbo",
                table: "role_permissions",
                newName: "ix_role_permissions_roles_id");

            migrationBuilder.RenameColumn(
                name: "keyword_id",
                schema: "dbo",
                table: "movie_keywords",
                newName: "keywords_id");

            migrationBuilder.RenameColumn(
                name: "genre_id",
                schema: "dbo",
                table: "movie_genres",
                newName: "genres_id");

            migrationBuilder.RenameColumn(
                name: "actor_id",
                schema: "dbo",
                table: "movie_actors",
                newName: "actors_id");

            migrationBuilder.RenameColumn(
                name: "actor_id",
                schema: "dbo",
                table: "episode_actors",
                newName: "actors_id");

            migrationBuilder.AddForeignKey(
                name: "fk_episode_actors_actors_actors_id",
                schema: "dbo",
                table: "episode_actors",
                column: "actors_id",
                principalSchema: "dbo",
                principalTable: "actors",
                principalColumn: "actor_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_movie_actors_actors_actors_id",
                schema: "dbo",
                table: "movie_actors",
                column: "actors_id",
                principalSchema: "dbo",
                principalTable: "actors",
                principalColumn: "actor_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_movie_genres_genres_genres_id",
                schema: "dbo",
                table: "movie_genres",
                column: "genres_id",
                principalSchema: "dbo",
                principalTable: "genres",
                principalColumn: "genre_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_movie_keywords_keywords_keywords_id",
                schema: "dbo",
                table: "movie_keywords",
                column: "keywords_id",
                principalSchema: "dbo",
                principalTable: "keywords",
                principalColumn: "keyword_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_role_permissions_permissions_permissions_id",
                schema: "dbo",
                table: "role_permissions",
                column: "permissions_id",
                principalSchema: "dbo",
                principalTable: "permissions",
                principalColumn: "permission_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_role_permissions_roles_roles_id",
                schema: "dbo",
                table: "role_permissions",
                column: "roles_id",
                principalSchema: "dbo",
                principalTable: "roles",
                principalColumn: "role_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_tv_serie_genres_genres_genres_id",
                schema: "dbo",
                table: "tv_serie_genres",
                column: "genres_id",
                principalSchema: "dbo",
                principalTable: "genres",
                principalColumn: "genre_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_tv_serie_keywords_keywords_keywords_id",
                schema: "dbo",
                table: "tv_serie_keywords",
                column: "keywords_id",
                principalSchema: "dbo",
                principalTable: "keywords",
                principalColumn: "keyword_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_permissions_permissions_permissions_id",
                schema: "dbo",
                table: "user_permissions",
                column: "permissions_id",
                principalSchema: "dbo",
                principalTable: "permissions",
                principalColumn: "permission_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_permissions_users_users_id",
                schema: "dbo",
                table: "user_permissions",
                column: "users_id",
                principalSchema: "dbo",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_roles_roles_roles_id",
                schema: "dbo",
                table: "user_roles",
                column: "roles_id",
                principalSchema: "dbo",
                principalTable: "roles",
                principalColumn: "role_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_roles_users_users_id",
                schema: "dbo",
                table: "user_roles",
                column: "users_id",
                principalSchema: "dbo",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
