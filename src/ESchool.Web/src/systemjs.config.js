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
      '@angular/animations': 'npm:@angular/animations/bundles/animations.umd.js',
      '@angular/animations/browser': 'npm:@angular/animations/bundles/animations-browser.umd.js',
      '@angular/animations/browser': 'npm:@angular/animations/bundles/animations-browser.umd.js',
      '@angular/core': 'npm:@angular/core/bundles/core.umd.js',
      '@angular/common': 'npm:@angular/common/bundles/common.umd.js',
      '@angular/compiler': 'npm:@angular/compiler/bundles/compiler.umd.js',
      '@angular/platform-browser': 'npm:@angular/platform-browser/bundles/platform-browser.umd.js',
      '@angular/platform-browser/animations': 'npm:@angular/platform-browser/bundles/platform-browser-animations.umd.js',
      '@angular/platform-browser-dynamic': 'npm:@angular/platform-browser-dynamic/bundles/platform-browser-dynamic.umd.js',
      '@angular/http': 'npm:@angular/http/bundles/http.umd.js',      
      '@angular/router': 'npm:@angular/router/bundles/router.umd.js',
      '@angular/forms': 'npm:@angular/forms/bundles/forms.umd.js',
      'ng2-bootstrap': 'npm:ng2-bootstrap/bundles/ngx-bootstrap.umd.js',
      'ng2-breadcrumb': 'npm:ng2-breadcrumb',
      'moment': 'npm:moment/moment.js',
      'ng2-bootstrap-modal': 'npm:ng2-bootstrap-modal',
      'ng2-ckeditor': 'npm:ng2-ckeditor',
      'ng2-select': 'npm:ng2-select',
      'ng2-tag-input': 'npm:ng2-tag-input',
      'ng2-material-dropdown': 'npm:ng2-material-dropdown',
      'ngx-rating': 'npm:ngx-rating',
      // other libraries
      'rxjs': 'npm:rxjs',
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
      translation: { main: 'index.js', defaultExtension: 'js' },
      'ng2-bootstrap-modal': {
        main: 'index.js',
        defaultExtension: 'js'
      },
      'ng2-breadcrumb': {
        main: 'ng2-breadcrumb.js',
        defaultExtension: 'js'
      },
      'ng2-ckeditor': {
        main: 'lib/index.js',
        defaultExtension: 'js'
      },
      'ng2-select': {
        main: 'index.js',
        defaultExtension: 'js'
      },
      'ng2-tag-input': {
        main: 'dist/ng2-tag-input.bundle.js',
        format: 'cjs'
      },
      'ng2-material-dropdown': {
        defaultExtension: 'js',
        main: 'dist/ng2-dropdown.bundle.js',
        format: 'cjs'
      },
      'ng2-tag-input/modules/components/tag-input.template.html': {
        defaultJSExtension: false
      },
      'ngx-rating': {
        defaultExtension: 'js',
        main: 'index.js'
      }
    }
  });
})(this);
