var addTopInfoElement = function () {
    var element = document.createElement("div");
    var topInfoText = "Documentation and API endpoints for electricity grid prices and tariffs for connected Distribution System Operators";
    element.className = "swagger-ui topinfo";
    element.innerText = topInfoText;
    document.body.insertBefore(element, document.body.firstChild);
};

function throttle(func, limit) { // Throttle function to prevent infinite loop execution
    let lastFunc;
    let lastRan;
    return function () {
        const context = this, args = arguments;
        if (!lastRan) {
            func.apply(context, args);
            lastRan = Date.now();
        } else {
            clearTimeout(lastFunc);
            lastFunc = setTimeout(function () {
                if ((Date.now() - lastRan) >= limit) {
                    func.apply(context, args);
                    lastRan = Date.now();
                }
            }, limit - (Date.now() - lastRan));
        }
    };
}

function updateSwaggerUI() {
    var link = document.querySelector(".swagger-ui .main .link");
    if (link) {
        link.href = link.href.replace("/ui", "");
        link.innerText = link.innerText.replace("/ui", "");
    }
}

if (document.readyState === "complete" || (document.readyState !== "loading" && !document.documentElement.doScroll)) {
    observeDocumentLoaded();
} else {
    document.addEventListener("DOMContentLoaded", observeDocumentLoaded);
}

function observeDocumentLoaded() {
    addTopInfoElement();

    var throttledUpdate = throttle(updateSwaggerUI, 10);

    var documentLoadedObserver = new MutationObserver(function (mutations, observer) {
        var mainContent = document.querySelector(".swagger-ui .main .link");
        if (mainContent) {
            console.log("link updated");
            throttledUpdate();
        }
    });

    documentLoadedObserver.observe(document.body, {
        childList: true,
        subtree: true
    });

    updateSwaggerUI();
}
