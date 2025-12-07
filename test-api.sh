#!/bin/bash

echo "==============================================="
echo "GrowDate API Testing Script"
echo "==============================================="
echo ""

API_BASE="http://localhost:5000"

echo "1. Testing Regions Endpoint"
echo "   GET /api/regions"
curl -s "$API_BASE/api/regions" | python3 -c "import sys, json; data=json.load(sys.stdin); print(f'   ✅ Found {len(data)} regions')"
echo ""

echo "2. Testing Crops Endpoint"
echo "   GET /api/crops"
curl -s "$API_BASE/api/crops" | python3 -c "import sys, json; data=json.load(sys.stdin); print(f'   ✅ Found {len(data)} crops')"
echo ""

echo "3. Testing Get Region by ID"
echo "   GET /api/regions/1"
curl -s "$API_BASE/api/regions/1" | python3 -c "import sys, json; data=json.load(sys.stdin); print(f'   ✅ {data[\"name\"]} ({data[\"climateZone\"]})')"
echo ""

echo "4. Testing Get Crop by ID"
echo "   GET /api/crops/1"
curl -s "$API_BASE/api/crops/1" | python3 -c "import sys, json; data=json.load(sys.stdin); print(f'   ✅ {data[\"name\"]} - {data[\"category\"]}')"
echo ""

echo "5. Testing Recommendations for Region"
echo "   GET /api/recommendations?regionId=1&selectedDate=2024-03-15"
curl -s "$API_BASE/api/recommendations?regionId=1&selectedDate=2024-03-15" | python3 -c "import sys, json; data=json.load(sys.stdin); print(f'   ✅ Found {len(data)} recommendations for California Central Valley')"
echo ""

echo "6. Testing Crops by Zone"
echo "   GET /api/crops/by-zone/Zone%209"
curl -s "$API_BASE/api/crops/by-zone/Zone%209" | python3 -c "import sys, json; data=json.load(sys.stdin); print(f'   ✅ Found {len(data)} crops suitable for Zone 9')"
echo ""

echo "7. Testing Crops by Category"
echo "   GET /api/crops/by-category/Vegetable"
curl -s "$API_BASE/api/crops/by-category/Vegetable" | python3 -c "import sys, json; data=json.load(sys.stdin); print(f'   ✅ Found {len(data)} vegetable crops')"
echo ""

echo "==============================================="
echo "All API tests completed successfully! 🎉"
echo "==============================================="
echo ""
echo "Access points:"
echo "  - API:      http://localhost:5000"
echo "  - Swagger:  http://localhost:5000/swagger"
echo "  - Frontend: http://localhost:5001"
echo ""
