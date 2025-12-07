// Simple 3D-style map visualization for region selection
// This is a simplified implementation - in production, you'd use Three.js or similar

class RegionMap {
    constructor(canvasId) {
        this.canvas = document.getElementById(canvasId);
        if (!this.canvas) return;
        
        this.ctx = this.canvas.getContext('2d');
        this.regions = [];
        this.selectedRegion = null;
        this.hoveredRegion = null;
        
        this.canvas.width = this.canvas.offsetWidth;
        this.canvas.height = this.canvas.offsetHeight;
        
        this.setupEventListeners();
    }
    
    setRegions(regions) {
        this.regions = regions.map(region => ({
            ...region,
            x: this.longitudeToX(region.longitude),
            y: this.latitudeToY(region.latitude),
            radius: 15
        }));
        this.draw();
    }
    
    longitudeToX(longitude) {
        return ((longitude + 180) / 360) * this.canvas.width;
    }
    
    latitudeToY(latitude) {
        return ((90 - latitude) / 180) * this.canvas.height;
    }
    
    setupEventListeners() {
        this.canvas.addEventListener('mousemove', (e) => {
            const rect = this.canvas.getBoundingClientRect();
            const x = e.clientX - rect.left;
            const y = e.clientY - rect.top;
            
            this.hoveredRegion = null;
            for (const region of this.regions) {
                const distance = Math.sqrt((x - region.x) ** 2 + (y - region.y) ** 2);
                if (distance < region.radius) {
                    this.hoveredRegion = region;
                    this.canvas.style.cursor = 'pointer';
                    break;
                }
            }
            
            if (!this.hoveredRegion) {
                this.canvas.style.cursor = 'default';
            }
            
            this.draw();
        });
        
        this.canvas.addEventListener('click', (e) => {
            if (this.hoveredRegion) {
                this.selectedRegion = this.hoveredRegion;
                this.draw();
                
                // Dispatch event for Blazor to handle
                const event = new CustomEvent('regionSelected', {
                    detail: { regionId: this.selectedRegion.id }
                });
                this.canvas.dispatchEvent(event);
            }
        });
    }
    
    draw() {
        // Clear canvas
        this.ctx.clearRect(0, 0, this.canvas.width, this.canvas.height);
        
        // Draw map background (simple gradient for water/land effect)
        const gradient = this.ctx.createLinearGradient(0, 0, 0, this.canvas.height);
        gradient.addColorStop(0, '#87CEEB');
        gradient.addColorStop(0.7, '#90EE90');
        gradient.addColorStop(1, '#DEB887');
        this.ctx.fillStyle = gradient;
        this.ctx.fillRect(0, 0, this.canvas.width, this.canvas.height);
        
        // Draw grid lines
        this.drawGrid();
        
        // Draw regions
        for (const region of this.regions) {
            this.drawRegion(region);
        }
    }
    
    drawGrid() {
        this.ctx.strokeStyle = 'rgba(255, 255, 255, 0.3)';
        this.ctx.lineWidth = 1;
        
        // Longitude lines
        for (let lon = -180; lon <= 180; lon += 30) {
            const x = this.longitudeToX(lon);
            this.ctx.beginPath();
            this.ctx.moveTo(x, 0);
            this.ctx.lineTo(x, this.canvas.height);
            this.ctx.stroke();
        }
        
        // Latitude lines
        for (let lat = -90; lat <= 90; lat += 30) {
            const y = this.latitudeToY(lat);
            this.ctx.beginPath();
            this.ctx.moveTo(0, y);
            this.ctx.lineTo(this.canvas.width, y);
            this.ctx.stroke();
        }
    }
    
    drawRegion(region) {
        const isSelected = this.selectedRegion && this.selectedRegion.id === region.id;
        const isHovered = this.hoveredRegion && this.hoveredRegion.id === region.id;
        
        // Draw marker
        this.ctx.beginPath();
        this.ctx.arc(region.x, region.y, region.radius, 0, Math.PI * 2);
        
        if (isSelected) {
            this.ctx.fillStyle = '#4CAF50';
            this.ctx.strokeStyle = '#2E7D32';
            this.ctx.lineWidth = 3;
        } else if (isHovered) {
            this.ctx.fillStyle = '#8BC34A';
            this.ctx.strokeStyle = '#4CAF50';
            this.ctx.lineWidth = 2;
        } else {
            this.ctx.fillStyle = '#FFC107';
            this.ctx.strokeStyle = '#FF9800';
            this.ctx.lineWidth = 2;
        }
        
        this.ctx.fill();
        this.ctx.stroke();
        
        // Draw label
        if (isHovered || isSelected) {
            this.ctx.fillStyle = '#333';
            this.ctx.font = 'bold 14px Arial';
            this.ctx.textAlign = 'center';
            this.ctx.textBaseline = 'bottom';
            this.ctx.fillText(region.name, region.x, region.y - region.radius - 5);
        }
    }
}

// Export for use in Blazor
window.RegionMap = RegionMap;
