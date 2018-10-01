"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
//import 'zone.js/dist/zone-node';
//import 'reflect-metadata';
//import { renderModule, renderModuleFactory } from '@angular/platform-server';
//import { APP_BASE_HREF } from '@angular/common';
//import { enableProdMode } from '@angular/core';
//import { provideModuleMap } from '@nguniversal/module-map-ngfactory-loader';
//import { createServerRenderer } from 'aspnet-prerendering';
var app_server_module_1 = require("./app/app.server.module");
exports.AppServerModule = app_server_module_1.AppServerModule;
//enableProdMode();
//export default createServerRenderer(params => {
//  const { AppServerModule, AppServerModuleNgFactory, LAZY_MODULE_MAP } = (module as any).exports;
//  const options = {
//    document: params.data.originalHtml,
//    url: params.url,
//    extraProviders: [
//      provideModuleMap(LAZY_MODULE_MAP),
//      { provide: APP_BASE_HREF, useValue: params.baseUrl },
//      { provide: 'BASE_URL', useValue: params.origin + params.baseUrl }
//    ]
//  };
//  const renderPromise = AppServerModuleNgFactory
//    ? /* AoT */ renderModuleFactory(AppServerModuleNgFactory, options)
//    : /* dev */ renderModule(AppServerModule, options);
//  return renderPromise.then(html => ({ html }));
//});
//# sourceMappingURL=main.server.js.map