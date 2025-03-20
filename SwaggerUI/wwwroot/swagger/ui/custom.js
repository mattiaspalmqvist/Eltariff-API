function redirectExternalRequestsToProxy(originalRequestUrl) {
    var redirictedUrl = originalRequestUrl;
    if (window.ui.getState().toJS().spec.json) {
        var servers = window.ui.getState().toJS().spec.json.servers;
        for (var s of servers) {
            var server = s.url;
            if (originalRequestUrl.includes(server)) {
                var version = originalRequestUrl.replace(server + "/", "").split("/")[0];
                var endpoint = originalRequestUrl.replace(server + "/" + version + "/", "");
                redirictedUrl = `/api/proxy?server=${encodeURIComponent(server)}&version=${encodeURIComponent(version)}&endpoint=${encodeURIComponent(endpoint)}`;
            }
        }
    }
    return redirictedUrl;
}

function addRequestInterceptor() {
    const originalFetch = window.fetch;
    window.fetch = async function (url, options) {
        redirectedUrl = redirectExternalRequestsToProxy(url);
        return originalFetch(redirectedUrl, options);
    };
};

function addTopInfoElement(rootElement) {
    var element = document.createElement("div");
    var topInfoText = "Documentation and API endpoints for electricity grid prices and tariffs for Distribution System Operators";
    element.className = "swagger-ui topinfo";
    element.innerText = topInfoText;
    rootElement.insertAdjacentElement("afterBegin", element);
};

function editSwaggerUI() {
    var link = document.querySelector(".swagger-ui .info .info__contact .link");
    if (link) {
        link.innerText = link.innerText.replace("Website", "GitHub");
    }
}

function onDocumentLoaded() {
    window.swaggerObjectStorage = {};

    var uiLoadedObserver = new MutationObserver(function (mutations, observer) {
        var swaggerUI = document.getElementById("swagger-ui");
        if (window.ui && swaggerUI) {
            observer.disconnect();
            addRequestInterceptor();
            addTopInfoElement(swaggerUI);
        }
    });

    var uiUpdatedObserver = new MutationObserver(function (mutations, observer) {
        var infoContent = document.querySelector(".swagger-ui .info .main");
        if (infoContent) {
            observer.disconnect();
            editSwaggerUI();
            setTimeout(() => {
                observer.observe(document.body, {
                    childList: true,
                    subtree: true
                })
            }, 100);
        }
    });

    uiLoadedObserver.observe(document.body, {
        childList: true,
        subtree: true
    });

    uiUpdatedObserver.observe(document.body, {
        childList: true,
        subtree: true
    });

    editSwaggerUI();
}

if (document.readyState === "complete" || (document.readyState !== "loading" && !document.documentElement.doScroll)) {
    onDocumentLoaded();
} else {
    document.addEventListener("DOMContentLoaded", onDocumentLoaded);
}
