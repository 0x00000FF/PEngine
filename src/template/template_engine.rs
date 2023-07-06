use std::{fs, collections::HashMap, str::FromStr};
use axum::{http::{Response, StatusCode}, body};
use regex::Regex;

struct TemplateEngine {
    view: String,
    status: StatusCode
}

impl TemplateEngine {
    pub fn init(template:&str) -> TemplateEngine {
        TemplateEngine::init_with_layout(template, "layout")
    }

    pub fn init_no_layout(template: &str) -> TemplateEngine {
        TemplateEngine::init_with_layout(template, "")
    }

    pub fn init_with_layout(template:&str, layout:&str) -> TemplateEngine {
        let mut init_view_data: String;
        let view_data = fs::read_to_string(format!("templates/{}.html", template)).unwrap();

        if layout.is_empty() {
           init_view_data = view_data; 
        } else {
            init_view_data = fs::read_to_string(format!("templates/{}.html", layout)).unwrap();
        
            let body_regex = Regex::new("@render_body").unwrap();
            
            if (body_regex.find_iter(&init_view_data).count() == 1) {
                let replaced = body_regex.replace(&init_view_data, view_data);
                init_view_data = String::from_str(&replaced).unwrap();
            }
        }

        TemplateEngine { view: init_view_data, status: StatusCode::OK }
    }

    pub fn set_view_model(self: &mut Self, vm: HashMap<String, Box<dyn ToString>>) -> &Self {
        let placeholder_regex = Regex::new("@([a-zA-Z_])").unwrap();
        let model_applied_view = placeholder_regex.replace_all(&self.view, |captures: &regex::Captures| {
            let captured = &captures[1];
            
            if vm.contains_key(captured) {
                vm.get(captured).unwrap().to_string()
            } else {
                "".to_owned()
            }
        });

        self.view = String::from_str(&model_applied_view).unwrap();

        self
    }

    pub fn set_status(self: &mut Self, status: StatusCode) -> &Self {
        self.status = status;
        self
    }

    pub fn build(self: &Self) -> Response<&[u8]> {
        let response = Response::new(self.view.as_bytes());



        response
    }
}