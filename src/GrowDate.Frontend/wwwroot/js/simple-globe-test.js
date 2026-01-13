// Enhanced Simple Globe Test
console.log('🧪 ENHANCED GLOBE TEST STARTING...');

window.testGlobeSimple = function() {
    console.log('=== STARTING ENHANCED GLOBE TEST ===');
    
    // Step 1: Check Three.js
    console.log('Step 1: Checking Three.js...');
    if (typeof THREE === 'undefined') {
        console.error('❌ THREE.js not loaded - CDN issue or script loading problem');
        alert('THREE.js library failed to load. Check your internet connection.');
        return false;
    }
    console.log('✅ THREE.js version:', THREE.REVISION);
    
    // Step 2: Check OrbitControls
    console.log('Step 2: Checking OrbitControls...');
    if (typeof THREE.OrbitControls === 'undefined') {
        console.error('❌ OrbitControls not loaded');
        alert('OrbitControls library failed to load.');
        return false;
    }
    console.log('✅ OrbitControls available');
    
    // Step 3: Find container
    console.log('Step 3: Finding globe container...');
    const container = document.getElementById('globe-container');
    if (!container) {
        console.error('❌ Globe container with id="globe-container" not found in DOM');
        alert('Globe container not found! Check if SelectRegion page loaded correctly.');
        return false;
    }
    
    const style = window.getComputedStyle(container);
    console.log('✅ Globe container found:', {
        width: container.clientWidth + 'px',
        height: container.clientHeight + 'px', 
        display: style.display,
        visibility: style.visibility,
        offsetParent: container.offsetParent ? 'visible' : 'hidden'
    });
    
    if (container.clientWidth === 0 || container.clientHeight === 0) {
        console.error('❌ Container has zero dimensions');
        alert('Globe container has zero size. Check CSS styling.');
        return false;
    }
    
    // Step 4: Clear container
    console.log('Step 4: Clearing container...');
    container.innerHTML = '';
    
    // Step 5: Try WebGL
    console.log('Step 5: Testing WebGL support...');
    let webglSupported = false;
    try {
        const testCanvas = document.createElement('canvas');
        const gl = testCanvas.getContext('webgl') || testCanvas.getContext('experimental-webgl');
        if (!gl) {
            console.warn('⚠️ WebGL not supported - will use 2D fallback');
            webglSupported = false;
        } else {
            console.log('✅ WebGL supported');
            webglSupported = true;
        }
    } catch (e) {
        console.warn('⚠️ WebGL test failed - will use 2D fallback:', e);
        webglSupported = false;
    }
    
    if (!webglSupported) {
        console.log('📍 Switching to 2D map fallback...');
        return window.testGlobe2D ? window.testGlobe2D() : create2DMap();
    }
    
    // Step 6: Create Three.js scene
    console.log('Step 6: Creating Three.js scene...');
    try {
        const scene = new THREE.Scene();
        scene.background = new THREE.Color(0x000011);
        
        const camera = new THREE.PerspectiveCamera(
            45, 
            container.clientWidth / container.clientHeight, 
            0.1, 
            1000
        );
        
        const renderer = new THREE.WebGLRenderer({ 
            antialias: true,
            alpha: true 
        });
        renderer.setSize(container.clientWidth, container.clientHeight);
        renderer.setPixelRatio(window.devicePixelRatio);
        
        console.log('✅ Scene, camera, renderer created');
        
        // Step 7: Add renderer to container
        console.log('Step 7: Adding renderer to DOM...');
        container.appendChild(renderer.domElement);
        
        // Step 8: Create simple geometry
        console.log('Step 8: Creating test geometry...');
        const geometry = new THREE.SphereGeometry(1, 32, 32);
        const material = new THREE.MeshBasicMaterial({ 
            color: 0x00ff00,
            wireframe: true
        });
        const sphere = new THREE.Mesh(geometry, material);
        scene.add(sphere);
        
        // Add some lights
        const ambientLight = new THREE.AmbientLight(0xffffff, 0.6);
        scene.add(ambientLight);
        
        camera.position.set(0, 0, 3);
        
        // Step 9: First render
        console.log('Step 9: First render...');
        renderer.render(scene, camera);
        console.log('✅ First render successful!');
        
        // Step 10: Animation
        console.log('Step 10: Starting animation...');
        function animate() {
            sphere.rotation.x += 0.01;
            sphere.rotation.y += 0.01;
            renderer.render(scene, camera);
            requestAnimationFrame(animate);
        }
        animate();
        
        console.log('🎉 SUCCESS! Globe test completed successfully!');
        console.log('You should see a green wireframe sphere rotating in the container.');
        
        // Add success message to container
        const successMsg = document.createElement('div');
        successMsg.style.position = 'absolute';
        successMsg.style.top = '10px';
        successMsg.style.left = '10px';
        successMsg.style.color = '#00ff00';
        successMsg.style.background = 'rgba(0,0,0,0.8)';
        successMsg.style.padding = '10px';
        successMsg.style.borderRadius = '5px';
        successMsg.innerHTML = '✅ 3D Globe Test: SUCCESS!<br/>Green sphere should be rotating below.';
        container.style.position = 'relative';
        container.appendChild(successMsg);
        
        return true;
        
    } catch (error) {
        console.error('❌ Three.js scene creation failed:', error);
        alert('Three.js failed to create 3D scene: ' + error.message);
        return false;
    }
};

// Auto-run test
document.addEventListener('DOMContentLoaded', function() {
    console.log('🧪 DOM loaded, will run globe test in 3 seconds...');
    setTimeout(function() {
        console.log('🧪 Auto-running globe test...');
        window.testGlobeSimple();
    }, 3000);
});

// 2D Map Fallback Function
function create2DMap() {
    console.log('🗺️ Creating 2D map fallback...');
    
    const container = document.getElementById('globe-container');
    if (!container) {
        console.error('❌ Globe container not found');
        return false;
    }
    
    container.innerHTML = `
        <div style="background: linear-gradient(180deg, #87CEEB 0%, #98FB98 100%); width: 100%; height: 400px; border-radius: 12px; position: relative; overflow: hidden; box-shadow: 0 4px 20px rgba(0,0,0,0.3);">
            <div style="position: absolute; top: 20px; left: 20px; color: #2c3e50; font-weight: bold; font-size: 18px;">🗺️ Interactive Region Map</div>
            <div style="position: absolute; top: 50px; left: 20px; color: #34495e; font-size: 14px;">⚠️ 3D mode unavailable - using 2D fallback</div>
            
            <!-- Continents as clickable regions -->
            <svg width="100%" height="100%" style="position: absolute; top: 0; left: 0;">
                <!-- North America -->
                <circle cx="25%" cy="35%" r="40" fill="#27ae60" opacity="0.8" stroke="#fff" stroke-width="2" 
                        style="cursor: pointer; transition: all 0.3s;" 
                        onmouseover="this.setAttribute('opacity', '1'); this.setAttribute('r', '45');" 
                        onmouseout="this.setAttribute('opacity', '0.8'); this.setAttribute('r', '40');" 
                        onclick="selectRegion('North America', this)">
                    <title>North America - Click to select</title>
                </circle>
                <text x="25%" y="35%" text-anchor="middle" dy="0.3em" fill="white" font-weight="bold" style="pointer-events: none; font-size: 12px;">NA</text>
                
                <!-- Europe -->
                <circle cx="55%" cy="30%" r="35" fill="#3498db" opacity="0.8" stroke="#fff" stroke-width="2" 
                        style="cursor: pointer; transition: all 0.3s;" 
                        onmouseover="this.setAttribute('opacity', '1'); this.setAttribute('r', '40');" 
                        onmouseout="this.setAttribute('opacity', '0.8'); this.setAttribute('r', '35');" 
                        onclick="selectRegion('Europe', this)">
                    <title>Europe - Click to select</title>
                </circle>
                <text x="55%" y="30%" text-anchor="middle" dy="0.3em" fill="white" font-weight="bold" style="pointer-events: none; font-size: 12px;">EU</text>
                
                <!-- Asia -->
                <circle cx="75%" cy="35%" r="50" fill="#e74c3c" opacity="0.8" stroke="#fff" stroke-width="2" 
                        style="cursor: pointer; transition: all 0.3s;" 
                        onmouseover="this.setAttribute('opacity', '1'); this.setAttribute('r', '55');" 
                        onmouseout="this.setAttribute('opacity', '0.8'); this.setAttribute('r', '50');" 
                        onclick="selectRegion('Asia', this)">
                    <title>Asia - Click to select</title>
                </circle>
                <text x="75%" y="35%" text-anchor="middle" dy="0.3em" fill="white" font-weight="bold" style="pointer-events: none; font-size: 12px;">AS</text>
                
                <!-- Africa -->
                <circle cx="50%" cy="60%" r="45" fill="#f39c12" opacity="0.8" stroke="#fff" stroke-width="2" 
                        style="cursor: pointer; transition: all 0.3s;" 
                        onmouseover="this.setAttribute('opacity', '1'); this.setAttribute('r', '50');" 
                        onmouseout="this.setAttribute('opacity', '0.8'); this.setAttribute('r', '45');" 
                        onclick="selectRegion('Africa', this)">
                    <title>Africa - Click to select</title>
                </circle>
                <text x="50%" y="60%" text-anchor="middle" dy="0.3em" fill="white" font-weight="bold" style="pointer-events: none; font-size: 12px;">AF</text>
                
                <!-- South America -->
                <circle cx="30%" cy="70%" r="35" fill="#9b59b6" opacity="0.8" stroke="#fff" stroke-width="2" 
                        style="cursor: pointer; transition: all 0.3s;" 
                        onmouseover="this.setAttribute('opacity', '1'); this.setAttribute('r', '40');" 
                        onmouseout="this.setAttribute('opacity', '0.8'); this.setAttribute('r', '35');" 
                        onclick="selectRegion('South America', this)">
                    <title>South America - Click to select</title>
                </circle>
                <text x="30%" y="70%" text-anchor="middle" dy="0.3em" fill="white" font-weight="bold" style="pointer-events: none; font-size: 12px;">SA</text>
                
                <!-- Oceania -->
                <circle cx="80%" cy="75%" r="25" fill="#1abc9c" opacity="0.8" stroke="#fff" stroke-width="2" 
                        style="cursor: pointer; transition: all 0.3s;" 
                        onmouseover="this.setAttribute('opacity', '1'); this.setAttribute('r', '30');" 
                        onmouseout="this.setAttribute('opacity', '0.8'); this.setAttribute('r', '25');" 
                        onclick="selectRegion('Oceania', this)">
                    <title>Oceania - Click to select</title>
                </circle>
                <text x="80%" y="75%" text-anchor="middle" dy="0.3em" fill="white" font-weight="bold" style="pointer-events: none; font-size: 11px;">OC</text>
            </svg>
            
            <div style="position: absolute; bottom: 15px; right: 20px; color: #2c3e50; font-size: 12px; background: rgba(255,255,255,0.8); padding: 8px 12px; border-radius: 6px;">
                🖱️ Click regions to select
            </div>
        </div>
        <div id="region-selection-info" style="margin-top: 10px; padding: 10px; background: #f8f9fa; border-radius: 6px; text-align: center; color: #666;">
            Click a region on the map to select it
        </div>
    `;
    
    // Add region selection function to global scope
    window.selectRegion = function(regionName, element) {
        console.log('🎯 Region selected:', regionName);
        
        // Reset all regions
        const circles = container.querySelectorAll('circle');
        circles.forEach(circle => {
            circle.setAttribute('stroke', '#fff');
            circle.setAttribute('stroke-width', '2');
        });
        
        // Highlight selected region
        element.setAttribute('stroke', '#2c3e50');
        element.setAttribute('stroke-width', '4');
        
        // Update info display
        const infoDiv = document.getElementById('region-selection-info');
        if (infoDiv) {
            infoDiv.innerHTML = `
                <div style="color: #27ae60; font-weight: bold; font-size: 16px;">✅ Selected: ${regionName}</div>
                <div style="margin-top: 5px; color: #666;">You can now proceed to view crops and planting recommendations for this region.</div>
            `;
        }
        
        // Trigger Blazor event if available
        if (window.DotNet && typeof window.DotNet.invokeMethodAsync === 'function') {
            try {
                window.DotNet.invokeMethodAsync('GrowDate.Frontend', 'OnRegionSelected', regionName);
            } catch (e) {
                console.log('Blazor integration not available:', e);
            }
        }
        
        alert(`🌍 Selected region: ${regionName}\\n\\nIn a full implementation, this would load crops and recommendations for ${regionName}.`);
    };
    
    console.log('✅ 2D map fallback created successfully');
    alert('🗺️ WebGL not available - created 2D interactive map instead!\\n\\nClick on the colored regions to select them.');
    return true;
}

// Make functions globally available
window.testGlobe2D = create2DMap;

console.log('🧪 Enhanced globe test ready.');