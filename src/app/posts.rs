use std::collections::HashMap;

use axum::{response::IntoResponse};

use crate::template::template_engine::TemplateEngine;

pub async fn main_page() -> impl IntoResponse {
    let mut vm:HashMap<String, Box<dyn ToString>> = HashMap::new();
    vm.insert("test".to_owned(), Box::new("hello my template engine!!".to_owned()));

    TemplateEngine::init("index")
        .set_view_model(vm)
        .build()
}

pub async fn post_read() {
    
}

pub async fn post_search() {

}

pub async fn post_write() {

}

pub async fn post_modify() {

}