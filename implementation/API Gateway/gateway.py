from fastapi import FastAPI, Request, Depends
from fastapi.responses import JSONResponse
import httpx
import redis.asyncio as redis  # Updated import
from fastapi_cache import FastAPICache
from fastapi_cache.backends.redis import RedisBackend
from fastapi_cache.decorator import cache

app = FastAPI()

# Microservices URLs
APPOINTMENT_SERVICE_URL = "http://localhost:5001/api"
ANALYTICS_SERVICE_URL = "http://localhost:5002/api"

# Dependency Injection for httpx client
async def get_http_client() -> httpx.AsyncClient:
    async with httpx.AsyncClient() as client:
        yield client

# Utility function to forward requests
async def forward_request(client: httpx.AsyncClient, target_url: str, path: str, request: Request):
    try:
        response = await client.request(
            method=request.method,
            url=f"{target_url}/{path}",
            content=await request.body(),
            headers=request.headers,
        )
        return JSONResponse(content=response.json(), status_code=response.status_code)
    except httpx.RequestError:
        return JSONResponse(content={"detail": "Error contacting the microservice"}, status_code=500)

# Proxy routes for Microservice 1 (GET requests only)
@app.api_route("/appointment/{path:path}", methods=["GET"])
@cache(expire=60)  # Cache GET requests for 60 seconds
async def appointment_service_proxy(path: str, request: Request, client: httpx.AsyncClient = Depends(get_http_client)):
    return await forward_request(client, APPOINTMENT_SERVICE_URL, f"Appointment/{path}", request)

@app.api_route("/appointmenthistory/{path:path}", methods=["GET"])
@cache(expire=60)  # Cache GET requests for 60 seconds
async def appointment_history_service_proxy(path: str, request: Request, client: httpx.AsyncClient = Depends(get_http_client)):
    return await forward_request(client, APPOINTMENT_SERVICE_URL, f"AppointmentHistory/{path}", request)

@app.api_route("/patient/{path:path}", methods=["GET"])
@cache(expire=60)  # Cache GET requests for 60 seconds
async def patient_service_proxy(path: str, request: Request, client: httpx.AsyncClient = Depends(get_http_client)):
    return await forward_request(client, APPOINTMENT_SERVICE_URL, f"Patient/{path}", request)

# Proxy routes for Microservice 2 (GET requests only)
@app.api_route("/doctor/{path:path}", methods=["GET"])
@cache(expire=60)  # Cache GET requests for 60 seconds
async def doctor_service_proxy(path: str, request: Request, client: httpx.AsyncClient = Depends(get_http_client)):
    return await forward_request(client, ANALYTICS_SERVICE_URL, f"Doctor/{path}", request)

@app.api_route("/operation/{path:path}", methods=["GET"])
@cache(expire=60)  # Cache GET requests for 60 seconds
async def operation_service_proxy(path: str, request: Request, client: httpx.AsyncClient = Depends(get_http_client)):
    return await forward_request(client, ANALYTICS_SERVICE_URL, f"Operation/{path}", request)

@app.get("/operationhistory")
@cache(expire=120)  # Cache GET requests for 120 seconds
async def get_operation_history(request: Request, client: httpx.AsyncClient = Depends(get_http_client)):
    start_date_time = request.query_params.get("startDateTime")
    end_date_time = request.query_params.get("endDateTime")

    if not start_date_time or not end_date_time:
        return {"detail": "Both startDateTime and endDateTime query parameters are required."}

    forwarded_url = f"{ANALYTICS_SERVICE_URL}/OperationHistory?startDateTime={start_date_time}&endDateTime={end_date_time}"
    return await forward_request(client, forwarded_url, request)

# Health check route
@app.get("/health")
async def health_check():
    return JSONResponse(content={"status": "OK", "message": "Gateway is running"}, status_code=200)

# Initialize Redis cache
@app.on_event("startup")
async def startup():
    redis_client = redis.Redis(host="localhost", port=6379, db=0, encoding="utf8", decode_responses=True)
    FastAPICache.init(RedisBackend(redis_client), prefix="fastapi-cache")

if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app, host="0.0.0.0", port=8000)
