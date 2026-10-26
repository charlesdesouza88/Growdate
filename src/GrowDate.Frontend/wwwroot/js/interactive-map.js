let blazorReference = null;

window.setBlazorReference = function (dotNetReference) {
    blazorReference = dotNetReference;
};

window.loadInteractiveMap = function (regions) {
    const container = document.getElementById('map-container');
    if (!container) {
        return false;
    }

    const markers = (regions || []).map((region) => {
        const x = ((region.longitude + 180) / 360) * 100;
        const y = ((90 - region.latitude) / 180) * 60;
        return {
            ...region,
            x: Math.min(96, Math.max(4, x)),
            y: Math.min(56, Math.max(4, y))
        };
    });

    const markerMarkup = markers.map((region) => `
        <g class="region-marker" data-region-id="${region.id}" onclick="selectRegionById(${region.id}, this)" style="cursor: pointer;">
            <circle cx="${region.x}" cy="${region.y}" r="2.4" fill="#2e7d32" stroke="#fff" stroke-width="0.6">
                <title>${escapeHtml(region.name)} (${escapeHtml(region.climateZone)})</title>
            </circle>
        </g>
    `).join('');

    container.innerHTML = `
        <div class="map-surface">
            <svg viewBox="0 0 100 60" preserveAspectRatio="xMidYMid meet" class="map-svg" role="img" aria-label="Growing regions">
                <rect width="100" height="60" fill="#d7e9f7"></rect>
                <path d="M8,22 C18,14 28,16 32,22 C36,18 44,16 48,22 C58,14 70,18 78,16 C86,20 90,28 86,36 C80,42 68,40 60,44 C48,48 40,42 30,44 C18,46 10,40 8,32 Z" fill="#c5e1a5" opacity="0.75"></path>
                ${markerMarkup}
            </svg>
            <p class="map-hint">Click a marker to select a growing region. Hover for the name.</p>
        </div>
    `;

    return true;
};

window.selectRegionById = function (regionId, element) {
    const container = document.getElementById('map-container');
    if (container) {
        container.querySelectorAll('circle').forEach((circle) => {
            circle.setAttribute('fill', '#2e7d32');
            circle.setAttribute('r', '2.4');
        });
        const circle = element ? element.querySelector('circle') : null;
        if (circle) {
            circle.setAttribute('fill', '#f9a825');
            circle.setAttribute('r', '3.2');
        }
    }

    if (blazorReference) {
        blazorReference.invokeMethodAsync('OnRegionSelected', regionId);
    }
};

window.disposeMap = function () {
    blazorReference = null;
};

function escapeHtml(value) {
    return String(value ?? '')
        .replace(/&/g, '&amp;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;')
        .replace(/"/g, '&quot;');
}
