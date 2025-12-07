// Enhanced debugging for globe initialization
console.log('=== Globe Debug Script Loaded ===');

// Check if THREE.js is available
console.log('THREE.js available:', typeof THREE !== 'undefined');
console.log('OrbitControls available:', typeof THREE !== 'undefined' && typeof THREE.OrbitControls !== 'undefined');

// Monitor when initGlobe is called
const originalInitGlobe = window.initGlobe;
window.initGlobe = function(...args) {
    console.log('=== initGlobe called ===');
    console.log('Arguments:', args);
    console.log('Regions data:', args[0]);
    console.log('Number of regions:', args[0]?.length);
    
    if (args[0] && args[0].length > 0) {
        console.log('First region sample:', {
            id: args[0][0].id,
            name: args[0][0].name,
            latitude: args[0][0].latitude,
            longitude: args[0][0].longitude
        });
    }
    
    try {
        const result = originalInitGlobe.apply(this, args);
        console.log('initGlobe completed successfully');
        return result;
    } catch (error) {
        console.error('Error in initGlobe:', error);
        throw error;
    }
};

// Check DOM when ready
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', checkGlobeContainer);
} else {
    checkGlobeContainer();
}

function checkGlobeContainer() {
    console.log('=== Checking Globe Container ===');
    const container = document.getElementById('globe-container');
    console.log('Container found:', !!container);
    if (container) {
        console.log('Container dimensions:', {
            width: container.clientWidth,
            height: container.clientHeight,
            offsetWidth: container.offsetWidth,
            offsetHeight: container.offsetHeight
        });
    }
}
