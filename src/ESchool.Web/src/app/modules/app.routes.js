"use strict";
var router_1 = require('@angular/router');
var home_component_1 = require('./home/home.component');
var restaurant_list_component_1 = require('./restaurants/restaurant-list.component');
var about_component_1 = require('./about/about.component');
var appRoutes = [
    { path: 'restaurants', component: restaurant_list_component_1.RestaurantListComponent },
    { path: 'about', component: about_component_1.AboutComponent },
    { path: '', component: home_component_1.HomeComponent }
];
exports.routing = router_1.RouterModule.forRoot(appRoutes);
//# sourceMappingURL=app.routes.js.map