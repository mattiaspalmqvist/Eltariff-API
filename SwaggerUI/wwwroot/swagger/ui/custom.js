var addTopInfoElement = function (rootElement) {
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

if (document.readyState === "complete" || (document.readyState !== "loading" && !document.documentElement.doScroll)) {
    onDocumentLoaded();
} else {
    document.addEventListener("DOMContentLoaded", onDocumentLoaded);
}

function onDocumentLoaded() {
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

    var uiLoadedObserver = new MutationObserver(function (mutations, observer) {
        var swaggerUI = document.getElementById("swagger-ui");
        if (swaggerUI) {
            observer.disconnect();
            addTopInfoElement(swaggerUI);
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
