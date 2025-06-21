window.onload = () => {
    const topbar = document.querySelector('.topbar-wrapper');
    if (topbar) {
        topbar.innerHTML = `
            <img alt="logo" src="./logo.png" style="height: 40px; filter: drop-shadow(0 0 5px cyan);" />
            <span>🚀 Event Organization API</span>
        `;
    }
};
