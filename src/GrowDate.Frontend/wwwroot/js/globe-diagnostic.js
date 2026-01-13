// Enhanced Globe Diagnostic Script
console.log('🌍 GLOBE DIAGNOSTIC STARTING...');

// Check all dependencies
function checkDependencies() {
    console.log('📋 Checking Dependencies:');
    console.log('- THREE.js available:', typeof THREE !== 'undefined');
    console.log('- OrbitControls available:', typeof THREE !== 'undefined' && typeof THREE.OrbitControls !== 'undefined');
    console.log('- Globe container exists:', !!document.getElementById('globe-container'));
    console.log('- Original initGlobe function exists:', typeof window.initGlobe === 'function');
}

// Monitor API calls
function monitorAPICall() {
    console.log('🌐 Testing API Connection...');
    
    fetch('/appsettings.json')
        .then(response => response.json())
        .then(config => {
            console.log('- Frontend config loaded:', config);
            
            return fetch(`${config.ApiBaseUrl}/api/regions`);
        })
        .then(response => {
            console.log('- API Response Status:', response.status);
            return response.json();
        })
        .then(regions => {
            console.log(`- Regions loaded: ${regions.length} found`);
            console.log('- Sample region:', regions[0]);
            
            // Try to initialize globe with real data
            if (typeof window.initGlobe === 'function') {
                console.log('🎯 Attempting to initialize globe...');
                try {
                    window.initGlobe(regions, null);
                    console.log('✅ Globe initialization completed!');
                } catch (error) {
                    console.error('❌ Globe initialization failed:', error);
                }
            }
        })
        .catch(error => {
            console.error('❌ API call failed:', error);
        });
}

// Check DOM state
function checkDOMState() {
    console.log('📄 DOM State:');
    const container = document.getElementById('globe-container');
    if (container) {
        console.log('- Container dimensions:', {
            width: container.clientWidth,
            height: container.clientHeight,
            visible: container.offsetParent !== null
        });
    } else {
        console.log('- Globe container: NOT FOUND');
    }
}

// Run diagnostics
function runDiagnostics() {
    checkDependencies();
    checkDOMState();
    monitorAPICall();
}

// Run when DOM is ready
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', runDiagnostics);
} else {
    runDiagnostics();
}

console.log('🔍 Globe diagnostics script loaded. Check output above.');