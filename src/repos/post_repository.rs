use async_trait::async_trait;

use crate::repos::repo_base::Repository;
use crate::models::post::Post;

use super::repo_base::RepositoryManager;

struct PostRepository {

}

#[async_trait]
impl Repository<Post, u64> for PostRepository {
    async fn get_one(&self, id: u64) -> Option<Post> {
        todo!()
    }

    async fn get_all(&self) -> Vec<Post> {
        todo!()
    }

    async fn insert(&self, entity: Post) -> Result<u64, sqlx::Error> {
        todo!()
    }

    async fn update(&self, entity: Post) -> Result<u64, sqlx::Error> {
        todo!()
    }

    async fn delete(&self, id: u64) -> Result<bool, sqlx::Error> {
        todo!()
    }
}