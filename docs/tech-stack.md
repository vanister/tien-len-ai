# Tiến Lên AI - Tech Stack Recommendation

## Executive Summary

For the web-based Tiến Lên game with AI capabilities, we recommend a microservices architecture splitting game engine (C#) from AI services (Python) to leverage the strengths of each technology while maintaining performance and scalability.

## Recommended Architecture

### Frontend Layer
- **Framework**: TypeScript + React 18
- **Styling**: TailwindCSS (consistent with project preferences)
- **Real-time**: WebSocket client for game state synchronization
- **Visualization**: Canvas or SVG for card animations and game board
- **Build Tool**: Vite or Create React App
- **Package Manager**: npm

### Backend Game Server
- **Language**: C# .NET 8
- **Framework**: ASP.NET Core Web API
- **Real-time**: SignalR for WebSocket connections
- **Database**: Entity Framework Core with SQL Server
- **Authentication**: JWT tokens or ASP.NET Identity
- **API Documentation**: Swagger/OpenAPI

### AI Engine (Microservice)
- **Language**: Python 3.11+
- **Framework**: FastAPI for HTTP API endpoints
- **ML Libraries**: 
  - NumPy for numerical computations
  - scikit-learn for basic ML algorithms
  - PyTorch or TensorFlow for neural networks
  - Custom MCTS implementation
- **Package Manager**: pip with requirements.txt
- **Deployment**: Docker containers

### Database Layer
- **Primary Database**: SQL Server (T-SQL stored procedures)
- **Schema**: Game history, player statistics, AI training data
- **Caching**: Redis for game state caching and AI position evaluation cache
- **Connection**: Entity Framework Core from C# backend

### Communication Layer
- **Frontend ↔ Game Server**: WebSocket (SignalR) + REST API
- **Game Server ↔ AI Service**: HTTP/REST or gRPC
- **Database**: Entity Framework Core with connection pooling

## Technology Justification

### Why C# for Game Engine?
1. **Performance**: Compiled language ideal for real-time game logic
2. **Type Safety**: Complex game rules benefit from strong typing
3. **Concurrency**: Excellent async/await support for multiplayer games
4. **Ecosystem**: SignalR provides robust WebSocket implementation
5. **Developer Preference**: Listed in user requirements
6. **Debugging**: Superior debugging experience for complex game logic

### Why Python for AI?
1. **ML Ecosystem**: Unmatched library support (NumPy, PyTorch, etc.)
2. **Rapid Prototyping**: Faster iteration on AI algorithms
3. **Community**: Extensive AI/ML community and resources
4. **Research Integration**: Easy to incorporate latest AI research
5. **Flexibility**: Simple to experiment with different AI approaches

### Why Microservices Split?
1. **Independent Scaling**: AI service can scale separately based on demand
2. **Technology Optimization**: Each service uses best-suited technology
3. **Development Velocity**: Teams can work independently on game logic vs AI
4. **Deployment Flexibility**: Different release cycles for game updates vs AI improvements
5. **Fault Isolation**: AI service issues don't crash the game server

## Implementation Phases

### Phase 1: Core Game Engine (C#)
- Immutable game state management
- Card system with fast comparisons
- Rule validation engine
- Legal move generation
- Unit test coverage > 90%

### Phase 2: Basic AI Service (Python)
- Simple rule-based AI via FastAPI
- MCTS implementation
- Position evaluation heuristics
- Integration with C# game server

### Phase 3: Web Frontend (TypeScript/React)
- Game board visualization
- Player interaction components
- SignalR integration for real-time updates
- Responsive design for mobile/desktop

### Phase 4: Advanced AI (Python)
- Neural network position evaluation
- Self-play training pipeline
- Advanced MCTS with learned evaluation
- Performance optimization

### Phase 5: Production Deployment
- Docker containerization
- Load balancing and scaling
- Monitoring and logging
- CI/CD pipelines

## Development Environment Setup

### Backend (C#)
```bash
# Create solution and projects
dotnet new sln -n TienLenAI
dotnet new webapi -n TienLenAI.GameEngine
dotnet new classlib -n TienLenAI.Core
dotnet new xunit -n TienLenAI.Tests

# Add project references
dotnet sln add **/*.csproj
```

### AI Service (Python)
```bash
# Virtual environment setup
python -m venv venv
source venv/bin/activate  # or venv\Scripts\activate on Windows

# Install dependencies
pip install fastapi uvicorn numpy scikit-learn pytest
```

### Frontend (TypeScript/React)
```bash
# Create React app with TypeScript
npx create-react-app tien-len-frontend --template typescript
cd tien-len-frontend
npm install @microsoft/signalr tailwindcss
```

## Data Flow Architecture

```
Frontend (React/TS)
    ↓ WebSocket/SignalR
Game Server (C# .NET)
    ↓ HTTP/gRPC  
AI Service (Python/FastAPI)
    ↓ 
AI Models & MCTS Engine
```

## Testing Strategy

### Unit Testing
- **C# Game Engine**: xUnit with 90%+ coverage
- **Python AI Service**: pytest with mock game states
- **TypeScript Frontend**: Jest + React Testing Library

### Integration Testing
- **API Testing**: Automated tests for all REST endpoints
- **WebSocket Testing**: SignalR connection and message flow
- **AI Integration**: End-to-end game simulations

### Performance Testing
- **Game Engine**: Benchmark legal move generation (target: <1ms)
- **AI Service**: MCTS simulations per second (target: 10,000+)
- **Full Stack**: Complete game response times (target: <200ms)

## Deployment Architecture

### Development
- **Local Development**: All services running locally
- **Docker Compose**: Orchestrate all services for testing

### Production
- **Game Server**: Azure App Service or AWS ECS
- **AI Service**: Containerized with auto-scaling
- **Database**: Azure SQL Database or AWS RDS
- **Frontend**: Static hosting (Vercel, Netlify, or CDN)
- **Cache**: Redis Cloud or managed Redis service

## Security Considerations

### Authentication
- JWT tokens for user sessions
- Rate limiting on AI service endpoints
- CORS configuration for frontend access

### Data Protection
- Encrypt sensitive user data
- Secure WebSocket connections (WSS)
- Input validation on all endpoints

## Monitoring and Observability

### Logging
- Structured logging with Serilog (C#)
- Python logging with structured output
- Centralized logging with ELK stack or similar

### Metrics
- Game completion rates
- AI decision latency
- WebSocket connection stability
- Database query performance

### Alerting
- AI service availability
- Database connection issues
- High error rates or latency spikes

## Cost Considerations

### Development Phase
- **Minimal Cost**: Local development, free tiers
- **Cloud Resources**: Basic tier databases and compute

### Production Phase
- **Variable Costs**: Scale with user base
- **AI Compute**: Most expensive component (GPU instances for training)
- **Optimization**: Cache AI evaluations, efficient algorithms

## Migration Path

If starting simple and growing:
1. **Phase 1**: Single C# application with embedded simple AI
2. **Phase 2**: Extract AI to separate Python service
3. **Phase 3**: Add advanced ML capabilities
4. **Phase 4**: Scale and optimize based on usage

## Conclusion

This tech stack provides:
- ✅ Optimal performance for real-time multiplayer games
- ✅ Best-in-class AI development capabilities  
- ✅ Scalable architecture for growth
- ✅ Maintainable codebase with proper separation of concerns
- ✅ Alignment with developer preferences and project requirements

The microservices approach allows leveraging C#'s strengths for game logic while utilizing Python's superior AI ecosystem, providing the best foundation for building a competitive Tiến Lên AI system.