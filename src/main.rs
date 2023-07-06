pub mod app;
pub mod models;
pub mod routes;
pub mod repos;
pub mod template;

use axum::{Router};

#[tokio::main]
async fn main() {
    let app = Router::new().nest("/", routes::make_routes());

    axum::Server::bind(&"127.0.0.1:5000".parse().unwrap())
        .serve(app.into_make_service())
        .await
        .unwrap()
}