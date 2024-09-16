# GamingLeaderboard
Score Leaderboard System Example For Gaming with Dotnet 8, PostgreSQL and Redis

# How to Install
Clone the repository
```bash
git clone https://github.com/Filiphasan/GamingLeaderboard.git
```

# Needs for running
- Docker

# How to Run
```bash
cd GamingLeaderboard
docker compose up -d
```

# Scalability and Performance
## Scalability
### Vertical Scaling
- Increase server resources for single instance project, increase CPU, RAM and Storage
- Advantage: Easy to scale

### Horizontal Scaling
- Add more instances for project and using a load balancer or api gateway for balance network traffic
- Advantage: Higher Scalability

## Performance
- Create Index for faster search in Users table for Username field, Unique.
- Create Index for faster search and grouping in UserScores table for UserId and Score fields.

# Error Handling and Data Consistency
## Data Consistency
- Lazy load redis sorted set for leaderboard, if redis has no data then load from database

# Security
- JWT: Use JWT for authentication, token expiration time is 30 minutes
- Hash: Use SHA256 hashing for password

# Logging & Monitoring
- Use Serilog for logging in ELK, all requests and responses are logged, this is useful for monitoring

# Could be improved by
- Use Event sourcing for user score and if redis has failed then load from event store
- Use CQRS and Event Sourcing for user score and leaderboard
- Use Elastic APM or Appoptics for professional monitoring and alerting

# Postman
- [Download Postman Collection](./Statics/Postman/Leaderboard%20API.postman_collection.json)