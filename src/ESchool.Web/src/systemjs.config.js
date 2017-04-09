/**
 * System configuration for Angular samples
 * Adjust as necessary for your application needs.
 */
(function (global) {
  System.config({
    paths: {
      // paths serve as alias
      'npm:': 'node_modules/'
    },
    // map tells the System loader where to look for things
    map: {
      // our app is within the app folder
      'app': 'app',
      'translation': 'app/shared/translate',
      // angular bundles
      '@angular/core': 'npm:@angular/core/bundles/core.umd.js',
      '@angular/common': 'npm:@angular/common/bundles/common.umd.js',
      '@angular/compiler': 'npm:@angular/compiler/bundles/compiler.umd.js',
      '@angular/platform-browser': 'npm:@angular/platform-browser/bundles/platform-browser.umd.js',
      '@angular/platform-browser-dynamic': 'npm:@angular/platform-browser-dynamic/bundles/platform-browser-dynamic.umd.js',
      '@angular/http': 'npm:@angular/http/bundles/http.umd.js',
      '@angular/router': 'npm:@angular/router/bundles/router.umd.js',
      '@angular/forms': 'npm:@angular/forms/bundles/forms.umd.js',
      'ng2-bootstrap': 'npm:ng2-bootstrap/bundles/ng2-bootstrap.umd.js',
      'ng2-breadcrumb': 'npm:ng2-breadcrumb',
      'moment': 'npm:moment/moment.js',
      'ng2-bootstrap-modal': 'npm:ng2-bootstrap-modal',
      "ng2-ckeditor": "npm:ng2-ckeditor",
      // other libraries
      'rxjs':                      'npm:rxjs',
      'angular-in-memory-web-api': 'npm:angular-in-memory-web-api/bundles/in-memory-web-api.umd.js'
    },
    // packages tells the System loader how to load when no filename and/or no extension
    packages: {
      app: {
        defaultExtension: 'js',
        meta: {
          './*.js': {
            loader: 'systemjs-angular-loader.js'
          }
        }
      },
      rxjs: {
        main: 'Rx.js',
        defaultExtension: 'js'
      },
      translation: { main: 'index.js',  defaultExtension: 'js' },
      'ng2-bootstrap-modal': {
        main: 'index.js',
        defaultExtension: 'js'
      },
      'ng2-breadcrumb': {
        main: 'ng2-breadcrumb.js',
        defaultExtension: 'js'
      },
      "ng2-ckeditor": {
        "main": "lib/index.js",
        "defaultExtension": "js",
      }
    }
  });
})(this);
