use std::{fs, collections::HashMap, str::FromStr, io::BufReader};
use axum::{http::{Response, StatusCode}, body::{self, Full}, response::IntoResponse};
use regex::Regex;

pub struct TemplateEngine {
    view_template: String,
    status: StatusCode,
    vm: HashMap<String, Box<dyn ToString>>
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
        
            let body_regex = Regex::new("@body").unwrap();
            
            if (body_regex.find_iter(&init_view_data).count() == 1) {
                let replaced = body_regex.replace(&init_view_data, view_data);
                init_view_data = String::from_str(&replaced).unwrap();
            } else {
                init_view_data = "No body defined".to_owned();
            }
        }

        TemplateEngine { view_template: init_view_data, status: StatusCode::OK, vm: HashMap::new() }
    }

    pub fn set_view_model(self: &mut Self, key: &str, value: Box<dyn ToString>) -> &mut Self {
        self.vm.insert( key.to_owned(), value );
        self
    }

    pub fn set_status(self: &mut Self, status: StatusCode) -> &mut Self {
        self.status = status;
        self
    }

    pub fn build(self: &Self) -> impl IntoResponse {
        let stream = self.parse_template();

        Response::builder()
            .status(self.status)
            .header("Content-Type", "text/html")
            .body(
                body::boxed(Full::from(stream))
            )
            .unwrap()
    }

    fn parse_template(self: &Self) -> Vec<u8> {
        let mut stream = vec![];
        
        let vt_size = self.view_template.bytes().count();
        let ch_bytes = self.view_template.as_bytes();

        let mut i = 0;
        loop {
            if i >= vt_size {
                break;
            } 

            let ch = ch_bytes[i];

            // check UTF-8
            if ch & 0b11000000 == 0b1100000 {
                stream.extend_from_slice(&ch_bytes[i..i+2]);
                i = i + 1;
            } else if ch & 0b11100000 == 0b11100000 {
                stream.extend_from_slice(&ch_bytes[i..i+3]);
                i = i + 2;
            } else if ch & 0b11110000 == 0b11110000 {
                stream.extend_from_slice(&ch_bytes[i..i+4]);
                i = i + 3;
            } else {
                let mut total_skip = 1;

                if ch == b'@' {
                    let next = ch_bytes[i+1];

                    if next == b'@' {           // verbatim @
                        stream.push(b'@');
                    } else if ch_bytes[i+1..i+3] == *b"if" {
                        stream.extend_from_slice(b"if block here");
                        total_skip += 2;

                    } else if ch_bytes[i+1..i+4] == *b"for" {
                        total_skip += 3;
                        
                        loop {
                            if ch_bytes[i + total_skip] == b' ' ||
                            ch_bytes[i + total_skip] == b'\t'  {
                                total_skip += 1;
                            } else if ch_bytes[i + total_skip] == b'(' {
                                total_skip += 1;
                                break;
                            } else {
                                todo!("expected parenthesis open");
                            }
                        }

                        let cursor_name_startpos = i + total_skip;
                        let mut cursor_name_length = 0;
                        
                        loop {
                            if ch_bytes[i + total_skip].is_ascii_alphabetic() || 
                            ch_bytes[i + total_skip].is_ascii_whitespace() {
                                cursor_name_length += 1;
                            } else if ch_bytes[i + total_skip] == b':' {
                                total_skip += 1;
                                break;
                            }
                            else {
                                todo!("unexpected char input or expected colon")
                            }

                            total_skip += 1;
                        }

                        let cursor_name = String::from_utf8(
                            ch_bytes[cursor_name_startpos..cursor_name_startpos+cursor_name_length].to_vec()
                        ).unwrap();

                        let collection_name_startpos = i + total_skip;
                        let mut collection_name_length = 0;
                        
                        loop {
                            if ch_bytes[i + total_skip].is_ascii_alphabetic() || 
                            ch_bytes[i + total_skip].is_ascii_whitespace() {
                                collection_name_length += 1;
                            } else if ch_bytes[i+total_skip] == b')' {
                                total_skip += 1;
                                break;
                            } else {
                                todo!("unexpected char input or expected parenthesis close")
                            }

                            total_skip += 1;
                        }

                        let collection_name = String::from_utf8(
                            ch_bytes[collection_name_startpos..collection_name_startpos+collection_name_length].to_vec()
                        ).unwrap();

                        println!("{} in {}", cursor_name.trim(), collection_name.trim());

                    } else if next == b'(' { // expression
                        let mut stage = 0;

                        loop {
                            if i + total_skip >= vt_size && stage > 0 {
                                todo!("error, parenthesis not match");
                            }

                            if ch_bytes[i+total_skip] == b'(' {
                                stage += 1;
                            } else if ch_bytes[i+total_skip] == b')' {
                                stage -= 1;

                                if stage == 0 {
                                    total_skip += 1;
                                    break;
                                }
                            }

                            total_skip += 1;
                        }

                        let id = String::from_utf8(ch_bytes[i+1..i+total_skip].to_vec()).unwrap();

                        stream.extend_from_slice(b"<h5>expression was here</h5>");

                        i += total_skip;
                        continue;
                        // "do eval and push eval result"
                    } else if next == b'{' { // block
                        let mut stage = 0;

                        loop {
                            if i + total_skip >= vt_size && stage > 0 {
                                todo!("error, parenthesis not match");
                            }

                            if ch_bytes[i+total_skip] == b'{' {
                                stage += 1;
                            } else if ch_bytes[i+total_skip] == b'}' {
                                stage -= 1;

                                if stage == 0 {
                                    total_skip += 1;
                                    break;
                                }
                            }

                            total_skip += 1;
                        }

                        stream.extend_from_slice(b"<h5>block was here</h5>");
                        i += total_skip;
                        continue;
                        // "do eval and push eval result";
                    } else {
                        loop {
                            if !ch_bytes[i+total_skip].is_ascii_alphabetic() {
                                break;
                            }

                            total_skip += 1;
                        }

                        let id = String::from_utf8(ch_bytes[i+1..i+total_skip].to_vec()).unwrap();
                        
                        if self.vm.contains_key(&id) {
                            stream.extend_from_slice(self.vm.get(&id).unwrap().to_string().as_bytes());
                        }
                    }

                    i += total_skip;

                    continue;
                } else {
                    stream.push(ch);
                }
            }

            i = i + 1;
        }

        stream
    }
}