// GrowDate Interactive 2D Map
console.log('🗺️ Interactive Map script loaded');

// Store Blazor reference
let blazorReference = null;

window.setBlazorReference = function(dotNetReference) {
    blazorReference = dotNetReference;
    console.log('✅ Blazor reference set successfully');
};

window.loadInteractiveMap = function() {
    console.log('=== LOADING INTERACTIVE 2D MAP ===');
    
    const container = document.getElementById('map-container');
    if (!container) {
        console.error('❌ Map container not found');
        alert('Map container not found on page');
        return false;
    }
    
    console.log('✅ Map container found, creating interactive map...');
    
    // Create the interactive map
    container.innerHTML = `
        <div style="background: linear-gradient(180deg, #87CEEB 0%, #4CAF50 100%); width: 100%; height: 100%; border-radius: 12px; position: relative; overflow: hidden;">
            <div style="position: absolute; top: 15px; left: 20px; color: #2c3e50; font-weight: bold; font-size: 20px; text-shadow: 2px 2px 4px rgba(0,0,0,0.3);">
                🌍 GrowDate Regions
            </div>
            <div style="position: absolute; top: 45px; left: 20px; color: #34495e; font-size: 14px; background: rgba(255,255,255,0.8); padding: 4px 8px; border-radius: 4px;">
                Select your growing region
            </div>
            
            <!-- Interactive SVG Map -->
            <svg width="100%" height="100%" style="position: absolute; top: 0; left: 0;">
                <!-- Background ocean -->
                <rect width="100%" height="100%" fill="url(#oceanGradient)" opacity="0.3"/>
                
                <!-- Gradient definitions -->
                <defs>
                    <radialGradient id="oceanGradient" cx="50%" cy="50%" r="50%">
                        <stop offset="0%" style="stop-color:#64B5F6;stop-opacity:1" />
                        <stop offset="100%" style="stop-color:#1976D2;stop-opacity:1" />
                    </radialGradient>
                </defs>
                
                <!-- North America -->
                <g onclick="selectRegion('North America', this)" style="cursor: pointer;">
                    <circle cx="20%" cy="30%" r="45" fill="#27ae60" opacity="0.9" stroke="#fff" stroke-width="3" 
                            style="transition: all 0.3s ease;" 
                            onmouseover="this.setAttribute('opacity', '1'); this.setAttribute('r', '50');" 
                            onmouseout="this.setAttribute('opacity', '0.9'); this.setAttribute('r', '45');">
                        <title>North America - Temperate crops, diverse growing seasons</title>
                    </circle>
                    <text x="20%" y="30%" text-anchor="middle" dy="0.3em" fill="white" font-weight="bold" style="pointer-events: none; font-size: 14px; text-shadow: 1px 1px 2px rgba(0,0,0,0.7);">NA</text>
                    <text x="20%" y="22%" text-anchor="middle" fill="white" style="pointer-events: none; font-size: 10px; text-shadow: 1px 1px 2px rgba(0,0,0,0.7);">North America</text>
                </g>
                
                <!-- Europe -->
                <g onclick="selectRegion('Europe', this)" style="cursor: pointer;">
                    <circle cx="50%" cy="25%" r="40" fill="#3498db" opacity="0.9" stroke="#fff" stroke-width="3" 
                            style="transition: all 0.3s ease;" 
                            onmouseover="this.setAttribute('opacity', '1'); this.setAttribute('r', '45');" 
                            onmouseout="this.setAttribute('opacity', '0.9'); this.setAttribute('r', '40');">
                        <title>Europe - Mediterranean and continental climates</title>
                    </circle>
                    <text x="50%" y="25%" text-anchor="middle" dy="0.3em" fill="white" font-weight="bold" style="pointer-events: none; font-size: 14px; text-shadow: 1px 1px 2px rgba(0,0,0,0.7);">EU</text>
                    <text x="50%" y="17%" text-anchor="middle" fill="white" style="pointer-events: none; font-size: 10px; text-shadow: 1px 1px 2px rgba(0,0,0,0.7);">Europe</text>
                </g>
                
                <!-- Asia -->
                <g onclick="selectRegion('Asia', this)" style="cursor: pointer;">
                    <circle cx="75%" cy="35%" r="55" fill="#e74c3c" opacity="0.9" stroke="#fff" stroke-width="3" 
                            style="transition: all 0.3s ease;" 
                            onmouseover="this.setAttribute('opacity', '1'); this.setAttribute('r', '60');" 
                            onmouseout="this.setAttribute('opacity', '0.9'); this.setAttribute('r', '55');">
                        <title>Asia - Diverse climates from tropical to temperate</title>
                    </circle>
                    <text x="75%" y="35%" text-anchor="middle" dy="0.3em" fill="white" font-weight="bold" style="pointer-events: none; font-size: 14px; text-shadow: 1px 1px 2px rgba(0,0,0,0.7);">AS</text>
                    <text x="75%" y="27%" text-anchor="middle" fill="white" style="pointer-events: none; font-size: 10px; text-shadow: 1px 1px 2px rgba(0,0,0,0.7);">Asia</text>
                </g>
                
                <!-- Africa -->
                <g onclick="selectRegion('Africa', this)" style="cursor: pointer;">
                    <circle cx="50%" cy="60%" r="48" fill="#f39c12" opacity="0.9" stroke="#fff" stroke-width="3" 
                            style="transition: all 0.3s ease;" 
                            onmouseover="this.setAttribute('opacity', '1'); this.setAttribute('r', '53');" 
                            onmouseout="this.setAttribute('opacity', '0.9'); this.setAttribute('r', '48');">
                        <title>Africa - Tropical and subtropical growing regions</title>
                    </circle>
                    <text x="50%" y="60%" text-anchor="middle" dy="0.3em" fill="white" font-weight="bold" style="pointer-events: none; font-size: 14px; text-shadow: 1px 1px 2px rgba(0,0,0,0.7);">AF</text>
                    <text x="50%" y="52%" text-anchor="middle" fill="white" style="pointer-events: none; font-size: 10px; text-shadow: 1px 1px 2px rgba(0,0,0,0.7);">Africa</text>
                </g>
                
                <!-- South America -->
                <g onclick="selectRegion('South America', this)" style="cursor: pointer;">
                    <circle cx="25%" cy="75%" r="40" fill="#9b59b6" opacity="0.9" stroke="#fff" stroke-width="3" 
                            style="transition: all 0.3s ease;" 
                            onmouseover="this.setAttribute('opacity', '1'); this.setAttribute('r', '45');" 
                            onmouseout="this.setAttribute('opacity', '0.9'); this.setAttribute('r', '40');">
                        <title>South America - Rich biodiversity and varied climates</title>
                    </circle>
                    <text x="25%" y="75%" text-anchor="middle" dy="0.3em" fill="white" font-weight="bold" style="pointer-events: none; font-size: 14px; text-shadow: 1px 1px 2px rgba(0,0,0,0.7);">SA</text>
                    <text x="25%" y="67%" text-anchor="middle" fill="white" style="pointer-events: none; font-size: 10px; text-shadow: 1px 1px 2px rgba(0,0,0,0.7);">South America</text>
                </g>
                
                <!-- Oceania -->
                <g onclick="selectRegion('Oceania', this)" style="cursor: pointer;">
                    <circle cx="80%" cy="75%" r="30" fill="#1abc9c" opacity="0.9" stroke="#fff" stroke-width="3" 
                            style="transition: all 0.3s ease;" 
                            onmouseover="this.setAttribute('opacity', '1'); this.setAttribute('r', '35');" 
                            onmouseout="this.setAttribute('opacity', '0.9'); this.setAttribute('r', '30');">
                        <title>Oceania - Island and coastal growing conditions</title>
                    </circle>
                    <text x="80%" y="75%" text-anchor="middle" dy="0.3em" fill="white" font-weight="bold" style="pointer-events: none; font-size: 12px; text-shadow: 1px 1px 2px rgba(0,0,0,0.7);">OC</text>
                    <text x="80%" y="67%" text-anchor="middle" fill="white" style="pointer-events: none; font-size: 9px; text-shadow: 1px 1px 2px rgba(0,0,0,0.7);">Oceania</text>
                </g>
            </svg>
            
            <!-- Instructions overlay -->
            <div style="position: absolute; bottom: 15px; right: 20px; background: rgba(255,255,255,0.95); padding: 10px 15px; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.2);">
                <div style="color: #2c3e50; font-size: 12px; font-weight: bold; margin-bottom: 4px;">🖱️ Interactive Controls:</div>
                <div style="color: #34495e; font-size: 11px;">• Hover regions for details</div>
                <div style="color: #34495e; font-size: 11px;">• Click to select region</div>
            </div>
        </div>
    `;
    
    // Add selection status area below the map
    const mapWrapper = container.parentElement;
    let statusDiv = document.getElementById('region-selection-status');
    if (!statusDiv) {
        statusDiv = document.createElement('div');
        statusDiv.id = 'region-selection-status';
        statusDiv.style.cssText = `
            margin-top: 15px; 
            padding: 15px; 
            background: linear-gradient(135deg, #f8f9fa, #e9ecef); 
            border-radius: 8px; 
            text-align: center; 
            border: 2px solid #dee2e6;
            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
        `;
        statusDiv.innerHTML = `
            <div style="color: #495057; font-size: 16px; font-weight: bold;">
                🌱 Select a region to view crops and planting recommendations
            </div>
            <div style="color: #6c757d; font-size: 14px; margin-top: 5px;">
                Each region has different growing seasons and suitable crops
            </div>
        `;
        mapWrapper.appendChild(statusDiv);
    }
    
    console.log('✅ Interactive map created successfully');
    return true;
};

// Region selection handler
window.selectRegion = function(regionName, element) {
    console.log('🎯 Region selected:', regionName);
    
    // Get the map container
    const container = document.getElementById('map-container');
    if (!container) return;
    
    // Reset all regions
    const circles = container.querySelectorAll('circle');
    circles.forEach(circle => {
        circle.setAttribute('stroke', '#fff');
        circle.setAttribute('stroke-width', '3');
    });
    
    // Highlight selected region
    const circle = element.querySelector('circle');
    if (circle) {
        circle.setAttribute('stroke', '#2c3e50');
        circle.setAttribute('stroke-width', '6');
        circle.setAttribute('opacity', '1');
    }
    
    // Update status display
    const statusDiv = document.getElementById('region-selection-status');
    if (statusDiv) {
        statusDiv.style.background = 'linear-gradient(135deg, #d4edda, #c3e6cb)';
        statusDiv.style.border = '2px solid #28a745';
        statusDiv.innerHTML = `
            <div style="color: #155724; font-size: 18px; font-weight: bold;">
                ✅ Selected: ${regionName}
            </div>
            <div style="color: #155724; font-size: 14px; margin-top: 8px;">
                🌾 Loading crops and planting recommendations for ${regionName}...
            </div>
            <div style="color: #6c757d; font-size: 12px; margin-top: 5px; font-style: italic;">
                Click another region to change your selection
            </div>
        `;
    }
    
    // Add ripple effect
    if (circle) {
        const currentR = circle.getAttribute('r');
        circle.setAttribute('r', parseInt(currentR) + 10);
        setTimeout(() => {
            circle.setAttribute('r', currentR);
        }, 200);
    }
    
    // Try to integrate with Blazor if available
    if (blazorReference) {
        try {
            blazorReference.invokeMethodAsync('OnMapRegionSelected', regionName);
            console.log('✅ Blazor integration: Region selection sent');
        } catch (e) {
            console.log('ℹ️ Blazor integration failed:', e.message);
        }
    } else {
        console.log('ℹ️ Blazor reference not available');
    }
    
    console.log(`✅ Region ${regionName} selected and highlighted`);
};

// Auto-load map when script is included
document.addEventListener('DOMContentLoaded', function() {
    console.log('🗺️ DOM loaded - Interactive map ready');
    
    // Auto-load the map if container exists
    setTimeout(() => {
        if (document.getElementById('map-container')) {
            console.log('🚀 Auto-loading interactive map...');
            window.loadInteractiveMap();
        }
    }, 1000);
});

console.log('🗺️ Interactive map script ready');