use std::{sync::Arc, borrow::Borrow};

use 
    sqlx::{postgres::{PgPool, PgQueryResult, PgPoolOptions, PgRow, PgArguments}, 
    query::{Query}, Postgres, Encode, Type};

pub trait Repository<TEntity, TId> {
    fn get_one(&self, id: TId) -> TEntity;
    fn get_all(&self) -> Vec<TEntity>;
    fn insert(&self, entity: TEntity) -> TId;
    fn update(&self, entity: TEntity) -> TId;
    fn delete(&self, id: TId) -> bool;
}

pub struct RepositoryManager {
    default_pool:Arc<PgPool>
}

impl RepositoryManager {
    pub async fn init(str: &str) -> Self {
        RepositoryManager { 
            default_pool: Arc::new(PgPoolOptions::new()
                            .max_connections(20)
                            .connect(str).await.unwrap()) }
    }

    pub async fn init_query<'a>(self: &'a Self, sql: &'a str) -> PEngineQuery<'a> {
        PEngineQuery::init(sqlx::query(&sql), Arc::clone(&self.default_pool)).await
    }
}

pub struct PEngineQuery<'a> {
    sql_query: Query<'a, Postgres, PgArguments>,
    pool: Arc<PgPool>
}

impl<'a> PEngineQuery<'a> {
    pub async fn init(query: Query<'a, Postgres, PgArguments>, pool: Arc<PgPool>) -> PEngineQuery<'_> {
        PEngineQuery { sql_query: query, pool }
    }

    pub fn bind<TArg>(mut self, arg: TArg) -> PEngineQuery<'a> 
    where TArg: 'a + Encode<'a, Postgres> + Send + Type<Postgres>
    {
        self.sql_query = self.sql_query.bind(arg);
        self
    }
    
    pub async fn fetch_all(self: Self) -> Result<Vec<PgRow>, sqlx::Error> {
        self.sql_query.fetch_all(self.pool.borrow()).await
    }

    pub async fn execute(self: Self) -> Result<PgQueryResult, sqlx::Error> {
        self.sql_query.execute(self.pool.borrow()).await
    }
}