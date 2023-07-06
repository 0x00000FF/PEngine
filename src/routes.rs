use std::{env, fs, default};

use axum::body::{self, Full};
use axum::extract::Path;
use axum::http::{Response, StatusCode};
use axum::response::IntoResponse;
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

    let etc_router = Router::new()
        .route("/static/*path", get(handle_static_request));

    Router::new()
        .nest("/", router)
        .nest("/", etc_router)
        .nest("/api", api_router)
}

static STATIC_FILE_ROOT:&str = "wwwroot";

pub fn default_plain_response(status_code:StatusCode, data: Vec<u8>) -> impl IntoResponse {
    Response::builder()
        .status(status_code)
        .body(body::boxed(Full::from(data)))
        .unwrap()
}

pub async fn handle_static_request(Path(path): Path<String>) -> impl IntoResponse {
    let working_path = std::fs::canonicalize(env::current_dir().unwrap()).unwrap();
    let canonicalized_path = std::fs::canonicalize(
        format!("{}/{}", STATIC_FILE_ROOT, path)
    ).unwrap();

    if canonicalized_path.starts_with(working_path) {
        match fs::read(canonicalized_path) {
            Ok(file_data) => default_plain_response(StatusCode::OK, file_data),
            Err(e) => default_plain_response(StatusCode::NOT_FOUND, vec![])
        }
    } else {
        default_plain_response(StatusCode::NOT_FOUND, vec![])
    }
}