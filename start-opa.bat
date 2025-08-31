@echo off
echo Starting OPA server with simple authentication policy...
echo.
echo Make sure you have OPA installed. If not, download from: https://www.openpolicyagent.org/docs/latest/get-started/
echo.

opa run --server --addr localhost:8181 OPA/

echo.
echo OPA server started at http://localhost:8181
echo Policy endpoint: http://localhost:8181/v1/data/container/auth/allow
echo.
echo Press Ctrl+C to stop the server
