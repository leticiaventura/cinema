const proxy = [
    {
        "/api/*": {
            "target": "https://localhost:44374/",
            "secure": false
        }
    }
];
module.exports = proxy;