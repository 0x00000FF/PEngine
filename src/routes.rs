use axum::{Router, routing::get, routing::post, routing::delete};

use crate::app::posts;
use crate::app::api;

pub fn make_routes() -> Router {
    let router = Router::new()
        .route("/", get(posts::main_page))
        .route("/read/:id", get(posts::post_read))
        .route("/search", get(posts::post_search))
        .route("/search/:keyword", get(posts::post_search))
        .route("/write", get(posts::post_write))
        .route("/modify/:id", get(posts::post_modify));

    let api_router = Router::new()
        .route("/api", get(api::api_version))
        .route("/api/post/update", post(api::posts::post_update))
        .route("/api/post/delete", delete(api::posts::post_delete));

    router.nest("/api", api_router)
}