use crate::repos::repo_base::Repository;
use crate::models::post::Post;

struct PostRepository {

}

impl Repository<Post, u64> for PostRepository {
    fn get_one(&self, id: u64) -> Post {
        todo!()
    }

    fn get_all(&self) -> Vec<Post> {
        todo!()
    }

    fn insert(&self, entity: Post) -> u64 {
        todo!()
    }

    fn update(&self, entity: Post) -> u64 {
        todo!()
    }

    fn delete(&self, id: u64) -> bool {
        todo!()
    }
}