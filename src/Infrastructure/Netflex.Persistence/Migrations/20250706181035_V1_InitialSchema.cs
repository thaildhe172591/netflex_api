using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Netflex.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class V1_InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "actors",
                schema: "dbo",
                columns: table => new
                {
                    actor_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    image = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    gender = table.Column<bool>(type: "boolean", nullable: false),
                    birth_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    biography = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_actors", x => x.actor_id);
                });

            migrationBuilder.CreateTable(
                name: "genres",
                schema: "dbo",
                columns: table => new
                {
                    genre_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_genres", x => x.genre_id);
                });

            migrationBuilder.CreateTable(
                name: "keywords",
                schema: "dbo",
                columns: table => new
                {
                    keyword_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_keywords", x => x.keyword_id);
                });

            migrationBuilder.CreateTable(
                name: "movies",
                schema: "dbo",
                columns: table => new
                {
                    movie_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    overview = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    poster_path = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    backdrop_path = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    video_url = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    country_iso = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    runtime = table.Column<int>(type: "integer", nullable: true),
                    release_date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_movies", x => x.movie_id);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                schema: "dbo",
                columns: table => new
                {
                    notification_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    content = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notifications", x => x.notification_id);
                });

            migrationBuilder.CreateTable(
                name: "permissions",
                schema: "dbo",
                columns: table => new
                {
                    permission_id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_permissions", x => x.permission_id);
                });

            migrationBuilder.CreateTable(
                name: "reports",
                schema: "dbo",
                columns: table => new
                {
                    report_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    reason = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    process = table.Column<string>(type: "text", nullable: false, defaultValue: "Open"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    last_modified_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reports", x => x.report_id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                schema: "dbo",
                columns: table => new
                {
                    role_id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "tv_series",
                schema: "dbo",
                columns: table => new
                {
                    tv_serie_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    overview = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    poster_path = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    backdrop_path = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    country_iso = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    first_air_date = table.Column<DateOnly>(type: "date", nullable: true),
                    last_air_date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tv_series", x => x.tv_serie_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "dbo",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: true),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "movie_actors",
                schema: "dbo",
                columns: table => new
                {
                    actor_id = table.Column<long>(type: "bigint", nullable: false),
                    movie_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_movie_actors", x => new { x.actor_id, x.movie_id });
                    table.ForeignKey(
                        name: "fk_movie_actors_actors_actor_id",
                        column: x => x.actor_id,
                        principalSchema: "dbo",
                        principalTable: "actors",
                        principalColumn: "actor_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_movie_actors_movies_movie_id",
                        column: x => x.movie_id,
                        principalSchema: "dbo",
                        principalTable: "movies",
                        principalColumn: "movie_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "movie_genres",
                schema: "dbo",
                columns: table => new
                {
                    genre_id = table.Column<long>(type: "bigint", nullable: false),
                    movie_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_movie_genres", x => new { x.genre_id, x.movie_id });
                    table.ForeignKey(
                        name: "fk_movie_genres_genres_genre_id",
                        column: x => x.genre_id,
                        principalSchema: "dbo",
                        principalTable: "genres",
                        principalColumn: "genre_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_movie_genres_movies_movie_id",
                        column: x => x.movie_id,
                        principalSchema: "dbo",
                        principalTable: "movies",
                        principalColumn: "movie_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "movie_keywords",
                schema: "dbo",
                columns: table => new
                {
                    keyword_id = table.Column<long>(type: "bigint", nullable: false),
                    movie_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_movie_keywords", x => new { x.keyword_id, x.movie_id });
                    table.ForeignKey(
                        name: "fk_movie_keywords_keywords_keyword_id",
                        column: x => x.keyword_id,
                        principalSchema: "dbo",
                        principalTable: "keywords",
                        principalColumn: "keyword_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_movie_keywords_movies_movie_id",
                        column: x => x.movie_id,
                        principalSchema: "dbo",
                        principalTable: "movies",
                        principalColumn: "movie_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "role_permissions",
                schema: "dbo",
                columns: table => new
                {
                    permission_id = table.Column<string>(type: "text", nullable: false),
                    role_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_permissions", x => new { x.permission_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_role_permissions_permissions_permission_id",
                        column: x => x.permission_id,
                        principalSchema: "dbo",
                        principalTable: "permissions",
                        principalColumn: "permission_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_role_permissions_roles_role_id",
                        column: x => x.role_id,
                        principalSchema: "dbo",
                        principalTable: "roles",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "episodes",
                schema: "dbo",
                columns: table => new
                {
                    episode_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    episode_number = table.Column<int>(type: "integer", nullable: false),
                    overview = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    video_url = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    runtime = table.Column<int>(type: "integer", nullable: true),
                    air_date = table.Column<DateOnly>(type: "date", nullable: true),
                    series_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_episodes", x => x.episode_id);
                    table.ForeignKey(
                        name: "fk_episodes_tv_series_series_id",
                        column: x => x.series_id,
                        principalSchema: "dbo",
                        principalTable: "tv_series",
                        principalColumn: "tv_serie_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tv_serie_genres",
                schema: "dbo",
                columns: table => new
                {
                    genre_id = table.Column<long>(type: "bigint", nullable: false),
                    tv_serie_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tv_serie_genres", x => new { x.genre_id, x.tv_serie_id });
                    table.ForeignKey(
                        name: "fk_tv_serie_genres_genres_genre_id",
                        column: x => x.genre_id,
                        principalSchema: "dbo",
                        principalTable: "genres",
                        principalColumn: "genre_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_tv_serie_genres_tv_series_tv_serie_id",
                        column: x => x.tv_serie_id,
                        principalSchema: "dbo",
                        principalTable: "tv_series",
                        principalColumn: "tv_serie_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tv_serie_keywords",
                schema: "dbo",
                columns: table => new
                {
                    keyword_id = table.Column<long>(type: "bigint", nullable: false),
                    tv_serie_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tv_serie_keywords", x => new { x.keyword_id, x.tv_serie_id });
                    table.ForeignKey(
                        name: "fk_tv_serie_keywords_keywords_keyword_id",
                        column: x => x.keyword_id,
                        principalSchema: "dbo",
                        principalTable: "keywords",
                        principalColumn: "keyword_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_tv_serie_keywords_tv_series_tv_serie_id",
                        column: x => x.tv_serie_id,
                        principalSchema: "dbo",
                        principalTable: "tv_series",
                        principalColumn: "tv_serie_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "follows",
                schema: "dbo",
                columns: table => new
                {
                    target_id = table.Column<string>(type: "text", nullable: false),
                    target_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_follows", x => new { x.user_id, x.target_id, x.target_type });
                    table.ForeignKey(
                        name: "fk_follows_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "dbo",
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reviews",
                schema: "dbo",
                columns: table => new
                {
                    target_id = table.Column<string>(type: "text", nullable: false),
                    target_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    rating = table.Column<int>(type: "integer", nullable: false),
                    comment = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    like_count = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reviews", x => new { x.user_id, x.target_id, x.target_type });
                    table.ForeignKey(
                        name: "fk_reviews_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "dbo",
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_logins",
                schema: "dbo",
                columns: table => new
                {
                    login_provider = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    provider_key = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_logins", x => new { x.login_provider, x.provider_key });
                    table.ForeignKey(
                        name: "fk_user_logins_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "dbo",
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_notifications",
                schema: "dbo",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    notification_id = table.Column<long>(type: "bigint", nullable: false),
                    have_read = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_notifications", x => new { x.user_id, x.notification_id });
                    table.ForeignKey(
                        name: "fk_user_notifications_notifications_notification_id",
                        column: x => x.notification_id,
                        principalSchema: "dbo",
                        principalTable: "notifications",
                        principalColumn: "notification_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_notifications_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "dbo",
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_permissions",
                schema: "dbo",
                columns: table => new
                {
                    permission_id = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_permissions", x => new { x.permission_id, x.user_id });
                    table.ForeignKey(
                        name: "fk_user_permissions_permissions_permission_id",
                        column: x => x.permission_id,
                        principalSchema: "dbo",
                        principalTable: "permissions",
                        principalColumn: "permission_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_permissions_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "dbo",
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                schema: "dbo",
                columns: table => new
                {
                    role_id = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_roles", x => new { x.role_id, x.user_id });
                    table.ForeignKey(
                        name: "fk_user_roles_roles_role_id",
                        column: x => x.role_id,
                        principalSchema: "dbo",
                        principalTable: "roles",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_roles_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "dbo",
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_sessions",
                schema: "dbo",
                columns: table => new
                {
                    user_session_id = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    device_id = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    device_info = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ip_address = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    refresh_hash = table.Column<string>(type: "text", nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_revoked = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_sessions", x => x.user_session_id);
                    table.ForeignKey(
                        name: "fk_user_sessions_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "dbo",
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "episode_actors",
                schema: "dbo",
                columns: table => new
                {
                    actor_id = table.Column<long>(type: "bigint", nullable: false),
                    episode_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_episode_actors", x => new { x.actor_id, x.episode_id });
                    table.ForeignKey(
                        name: "fk_episode_actors_actors_actor_id",
                        column: x => x.actor_id,
                        principalSchema: "dbo",
                        principalTable: "actors",
                        principalColumn: "actor_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_episode_actors_episodes_episode_id",
                        column: x => x.episode_id,
                        principalSchema: "dbo",
                        principalTable: "episodes",
                        principalColumn: "episode_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_episode_actors_episode_id",
                schema: "dbo",
                table: "episode_actors",
                column: "episode_id");

            migrationBuilder.CreateIndex(
                name: "ix_episodes_series_id",
                schema: "dbo",
                table: "episodes",
                column: "series_id");

            migrationBuilder.CreateIndex(
                name: "ix_genres_name",
                schema: "dbo",
                table: "genres",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_keywords_name",
                schema: "dbo",
                table: "keywords",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_movie_actors_movie_id",
                schema: "dbo",
                table: "movie_actors",
                column: "movie_id");

            migrationBuilder.CreateIndex(
                name: "ix_movie_genres_movie_id",
                schema: "dbo",
                table: "movie_genres",
                column: "movie_id");

            migrationBuilder.CreateIndex(
                name: "ix_movie_keywords_movie_id",
                schema: "dbo",
                table: "movie_keywords",
                column: "movie_id");

            migrationBuilder.CreateIndex(
                name: "ix_permissions_name",
                schema: "dbo",
                table: "permissions",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_role_permissions_role_id",
                schema: "dbo",
                table: "role_permissions",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_roles_name",
                schema: "dbo",
                table: "roles",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_tv_serie_genres_tv_serie_id",
                schema: "dbo",
                table: "tv_serie_genres",
                column: "tv_serie_id");

            migrationBuilder.CreateIndex(
                name: "ix_tv_serie_keywords_tv_serie_id",
                schema: "dbo",
                table: "tv_serie_keywords",
                column: "tv_serie_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_logins_user_id",
                schema: "dbo",
                table: "user_logins",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_notifications_notification_id",
                schema: "dbo",
                table: "user_notifications",
                column: "notification_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_permissions_user_id",
                schema: "dbo",
                table: "user_permissions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_roles_user_id",
                schema: "dbo",
                table: "user_roles",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_sessions_user_id_device_id",
                schema: "dbo",
                table: "user_sessions",
                columns: new[] { "user_id", "device_id" },
                unique: true,
                filter: "is_revoked = false");

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                schema: "dbo",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "episode_actors",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "follows",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "movie_actors",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "movie_genres",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "movie_keywords",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "reports",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "reviews",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "role_permissions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "tv_serie_genres",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "tv_serie_keywords",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "user_logins",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "user_notifications",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "user_permissions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "user_roles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "user_sessions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "episodes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "actors",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "movies",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "genres",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "keywords",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "notifications",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "permissions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "roles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "users",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "tv_series",
                schema: "dbo");
        }
    }
}
