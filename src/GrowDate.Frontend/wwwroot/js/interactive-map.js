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
            x: clamp(x, 8, 92),
            y: clamp(y, 10, 48),
            label: shortName(region.name)
        };
    });

    spreadMarkers(markers, 14);

    const markerMarkup = markers.map((region) => {
        return `
        <g class="region-marker" data-region-id="${region.id}" onclick="selectRegionById(${region.id}, this)" style="cursor: pointer;">
            <circle class="marker-hit" cx="${region.x}" cy="${region.y}" r="5.6" fill="transparent"></circle>
            <circle class="marker-dot" cx="${region.x}" cy="${region.y}" r="2.6" fill="#2f5d3a" stroke="#fffcf6" stroke-width="0.8">
                <title>${escapeHtml(region.name)} (${escapeHtml(region.climateZone)})</title>
            </circle>
            <text class="marker-label" x="${region.x}" y="${region.y + 5.6}" text-anchor="middle"
                  fill="#1c2a1f" font-size="3.1" font-family="Source Sans 3, Segoe UI, sans-serif">${escapeHtml(region.label)}</text>
        </g>`;
    }).join('');

    container.innerHTML = `
        <div class="map-surface">
            <svg viewBox="0 0 100 60" preserveAspectRatio="xMidYMid meet" class="map-svg" role="img" aria-label="Growing regions">
                <rect width="100" height="60" fill="#e7efe4" pointer-events="none"></rect>
                <path d="M8,22 C18,14 28,16 32,22 C36,18 44,16 48,22 C58,14 70,18 78,16 C86,20 90,28 86,36 C80,42 68,40 60,44 C48,48 40,42 30,44 C18,46 10,40 8,32 Z" fill="#c5d6bf" opacity="0.9" pointer-events="none"></path>
                ${markerMarkup}
            </svg>
            <p class="map-hint">Click a marker or its label to select a growing region.</p>
        </div>
    `;

    return true;
};

window.selectRegionById = function (regionId, element) {
    const container = document.getElementById('map-container');
    if (container) {
        container.querySelectorAll('.marker-dot').forEach((circle) => {
            circle.setAttribute('fill', '#2f5d3a');
            circle.setAttribute('r', '2.6');
        });
        container.querySelectorAll('.marker-label').forEach((label) => {
            label.setAttribute('fill', '#1c2a1f');
            label.setAttribute('font-weight', '400');
        });
        const circle = element ? element.querySelector('.marker-dot') : null;
        const label = element ? element.querySelector('.marker-label') : null;
        if (circle) {
            circle.setAttribute('fill', '#c47b2a');
            circle.setAttribute('r', '3.4');
        }
        if (label) {
            label.setAttribute('fill', '#c47b2a');
            label.setAttribute('font-weight', '700');
        }
    }

    if (blazorReference) {
        blazorReference.invokeMethodAsync('OnRegionSelected', regionId);
    }
};

window.disposeMap = function () {
    blazorReference = null;
};

function shortName(name) {
    const known = {
        'California Central Valley': 'California',
        'Texas Hill Country': 'Texas',
        'São Paulo State': 'São Paulo',
        'Pacific Northwest': 'Pacific NW'
    };
    return known[name] || String(name ?? '');
}

function clamp(value, min, max) {
    return Math.min(max, Math.max(min, value));
}

function spreadMarkers(markers, minDist) {
    for (let iter = 0; iter < 60; iter++) {
        let moved = false;
        for (let i = 0; i < markers.length; i++) {
            for (let j = i + 1; j < markers.length; j++) {
                const dx = markers[j].x - markers[i].x;
                const dy = markers[j].y - markers[i].y;
                const dist = Math.hypot(dx, dy) || 0.01;
                if (dist < minDist) {
                    const push = (minDist - dist) / 2;
                    const nx = dx / dist;
                    const ny = dy / dist;
                    markers[i].x = clamp(markers[i].x - nx * push, 8, 92);
                    markers[i].y = clamp(markers[i].y - ny * push, 10, 48);
                    markers[j].x = clamp(markers[j].x + nx * push, 8, 92);
                    markers[j].y = clamp(markers[j].y + ny * push, 10, 48);
                    moved = true;
                }
            }
        }
        if (!moved) {
            break;
        }
    }
}

function escapeHtml(value) {
    return String(value ?? '')
        .replace(/&/g, '&amp;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;')
        .replace(/"/g, '&quot;');
}
