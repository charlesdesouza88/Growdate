// 3D Globe Visualization with Three.js
let scene, camera, renderer, globe, controls;
let regions = [];
let markers = [];
let selectedRegion = null;

// Initialize the 3D globe
window.initGlobe = function(regionData, onRegionClick) {
    regions = regionData;
    
    const container = document.getElementById('globe-container');
    if (!container) return;
    
    // Scene setup
    scene = new THREE.Scene();
    scene.background = new THREE.Color(0x0a0a0a);
    
    // Camera setup
    camera = new THREE.PerspectiveCamera(
        45,
        container.clientWidth / container.clientHeight,
        0.1,
        1000
    );
    camera.position.z = 2.5;
    
    // Renderer setup
    renderer = new THREE.WebGLRenderer({ antialias: true, alpha: true });
    renderer.setSize(container.clientWidth, container.clientHeight);
    renderer.setPixelRatio(window.devicePixelRatio);
    container.appendChild(renderer.domElement);
    
    // Create globe
    createGlobe();
    
    // Add region markers
    addRegionMarkers(onRegionClick);
    
    // Lighting
    const ambientLight = new THREE.AmbientLight(0xffffff, 0.6);
    scene.add(ambientLight);
    
    const directionalLight = new THREE.DirectionalLight(0xffffff, 0.8);
    directionalLight.position.set(5, 3, 5);
    scene.add(directionalLight);
    
    // Controls for rotation
    controls = new THREE.OrbitControls(camera, renderer.domElement);
    controls.enableDamping = true;
    controls.dampingFactor = 0.05;
    controls.minDistance = 1.5;
    controls.maxDistance = 4;
    controls.enablePan = false;
    
    // Handle window resize
    window.addEventListener('resize', onWindowResize);
    
    // Raycaster for click detection
    const raycaster = new THREE.Raycaster();
    const mouse = new THREE.Vector2();
    
    renderer.domElement.addEventListener('click', (event) => {
        const rect = renderer.domElement.getBoundingClientRect();
        mouse.x = ((event.clientX - rect.left) / rect.width) * 2 - 1;
        mouse.y = -((event.clientY - rect.top) / rect.height) * 2 + 1;
        
        raycaster.setFromCamera(mouse, camera);
        const intersects = raycaster.intersectObjects(markers);
        
        if (intersects.length > 0) {
            const clickedMarker = intersects[0].object;
            if (clickedMarker.userData.region) {
                selectRegion(clickedMarker.userData.region, onRegionClick);
            }
        }
    });
    
    // Start animation
    animate();
};

function createGlobe() {
    // Create sphere geometry
    const geometry = new THREE.SphereGeometry(1, 64, 64);
    
    // Create material with Earth-like appearance
    const material = new THREE.MeshPhongMaterial({
        color: 0x2563eb,
        emissive: 0x112244,
        specular: 0x333333,
        shininess: 25,
        transparent: true,
        opacity: 0.9
    });
    
    globe = new THREE.Mesh(geometry, material);
    scene.add(globe);
    
    // Add wireframe overlay for continents effect
    const wireframeGeometry = new THREE.SphereGeometry(1.01, 32, 32);
    const wireframeMaterial = new THREE.MeshBasicMaterial({
        color: 0x10b981,
        wireframe: true,
        transparent: true,
        opacity: 0.3
    });
    const wireframe = new THREE.Mesh(wireframeGeometry, wireframeMaterial);
    scene.add(wireframe);
    
    // Add atmospheric glow
    const glowGeometry = new THREE.SphereGeometry(1.15, 32, 32);
    const glowMaterial = new THREE.MeshBasicMaterial({
        color: 0x3b82f6,
        transparent: true,
        opacity: 0.15,
        side: THREE.BackSide
    });
    const glow = new THREE.Mesh(glowGeometry, glowMaterial);
    scene.add(glow);
}

function addRegionMarkers(onRegionClick) {
    regions.forEach(region => {
        // Convert lat/long to 3D coordinates
        const phi = (90 - region.latitude) * (Math.PI / 180);
        const theta = (region.longitude + 180) * (Math.PI / 180);
        
        const x = -(1.02 * Math.sin(phi) * Math.cos(theta));
        const y = 1.02 * Math.cos(phi);
        const z = 1.02 * Math.sin(phi) * Math.sin(theta);
        
        // Create marker (pin shape)
        const markerGroup = new THREE.Group();
        
        // Pin body
        const pinGeometry = new THREE.CylinderGeometry(0.02, 0.01, 0.1, 8);
        const pinMaterial = new THREE.MeshPhongMaterial({
            color: 0x10b981,
            emissive: 0x059669
        });
        const pin = new THREE.Mesh(pinGeometry, pinMaterial);
        pin.rotation.x = Math.PI / 2;
        markerGroup.add(pin);
        
        // Pin head (sphere)
        const headGeometry = new THREE.SphereGeometry(0.03, 16, 16);
        const headMaterial = new THREE.MeshPhongMaterial({
            color: 0x34d399,
            emissive: 0x10b981
        });
        const head = new THREE.Mesh(headGeometry, headMaterial);
        head.position.z = 0.05;
        markerGroup.add(head);
        
        // Add glow effect
        const glowGeometry = new THREE.SphereGeometry(0.05, 16, 16);
        const glowMaterial = new THREE.MeshBasicMaterial({
            color: 0x10b981,
            transparent: true,
            opacity: 0.4
        });
        const glowSphere = new THREE.Mesh(glowGeometry, glowMaterial);
        glowSphere.position.z = 0.05;
        markerGroup.add(glowSphere);
        
        // Position marker on globe
        markerGroup.position.set(x, y, z);
        markerGroup.lookAt(0, 0, 0);
        
        // Store region data
        markerGroup.userData.region = region;
        markerGroup.userData.originalColor = 0x10b981;
        
        markers.push(markerGroup);
        scene.add(markerGroup);
        
        // Add text label
        createTextLabel(region.name, x * 1.2, y * 1.2, z * 1.2);
    });
}

function createTextLabel(text, x, y, z) {
    const canvas = document.createElement('canvas');
    const context = canvas.getContext('2d');
    canvas.width = 256;
    canvas.height = 64;
    
    context.fillStyle = 'rgba(255, 255, 255, 0.9)';
    context.font = 'Bold 20px Arial';
    context.textAlign = 'center';
    context.fillText(text, 128, 40);
    
    const texture = new THREE.CanvasTexture(canvas);
    const material = new THREE.SpriteMaterial({
        map: texture,
        transparent: true
    });
    
    const sprite = new THREE.Sprite(material);
    sprite.position.set(x, y, z);
    sprite.scale.set(0.5, 0.125, 1);
    scene.add(sprite);
}

function selectRegion(region, callback) {
    // Reset previous selection
    markers.forEach(marker => {
        marker.children.forEach(child => {
            if (child.material) {
                child.material.color.setHex(marker.userData.originalColor);
                child.material.emissive.setHex(0x059669);
            }
        });
    });
    
    // Highlight selected region
    const selectedMarker = markers.find(m => m.userData.region.id === region.id);
    if (selectedMarker) {
        selectedMarker.children.forEach(child => {
            if (child.material) {
                child.material.color.setHex(0xfbbf24);
                child.material.emissive.setHex(0xf59e0b);
            }
        });
        
        // Rotate camera to face the region
        const targetPosition = selectedMarker.position.clone().multiplyScalar(2.5);
        animateCameraToPosition(targetPosition);
    }
    
    selectedRegion = region;
    
    // Call the callback to update the Blazor component
    if (callback) {
        callback.invokeMethodAsync('OnRegionSelected', region.id);
    }
}

function animateCameraToPosition(targetPosition) {
    const startPosition = camera.position.clone();
    const duration = 1000; // 1 second
    const startTime = Date.now();
    
    function animate() {
        const elapsed = Date.now() - startTime;
        const progress = Math.min(elapsed / duration, 1);
        
        // Easing function (ease-in-out)
        const eased = progress < 0.5
            ? 2 * progress * progress
            : -1 + (4 - 2 * progress) * progress;
        
        camera.position.lerpVectors(startPosition, targetPosition, eased);
        camera.lookAt(0, 0, 0);
        
        if (progress < 1) {
            requestAnimationFrame(animate);
        }
    }
    
    animate();
}

function animate() {
    requestAnimationFrame(animate);
    
    // Slow auto-rotation
    if (globe && !controls.autoRotate) {
        globe.rotation.y += 0.001;
    }
    
    // Pulse effect on markers
    const time = Date.now() * 0.001;
    markers.forEach((marker, index) => {
        const glow = marker.children[2]; // The glow sphere
        if (glow) {
            const scale = 1 + Math.sin(time * 2 + index) * 0.2;
            glow.scale.set(scale, scale, scale);
        }
    });
    
    controls.update();
    renderer.render(scene, camera);
}

function onWindowResize() {
    const container = document.getElementById('globe-container');
    if (!container) return;
    
    camera.aspect = container.clientWidth / container.clientHeight;
    camera.updateProjectionMatrix();
    renderer.setSize(container.clientWidth, container.clientHeight);
}

// Cleanup function
window.disposeGlobe = function() {
    if (renderer) {
        renderer.dispose();
        const container = document.getElementById('globe-container');
        if (container && renderer.domElement) {
            container.removeChild(renderer.domElement);
        }
    }
    window.removeEventListener('resize', onWindowResize);
};
