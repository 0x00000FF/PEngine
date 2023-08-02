#!/bin/sh

envs="-e MYSQL_DATABASE=penginedev -e MYSQL_USER=pengine -e MYSQL_PASSWORD=pengine -e MYSQL_RANDOM_ROOT_PASSWORD=yes"
ports="-p 30000:3306"

docker run --name pengine-dev $envs $ports mysql:8 