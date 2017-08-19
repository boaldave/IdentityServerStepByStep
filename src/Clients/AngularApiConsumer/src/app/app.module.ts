import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AuthModule, OidcSecurityService, OpenIDImplicitFlowConfiguration } from 'angular-auth-oidc-client';

import { AppComponent } from './app.component';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AuthModule.forRoot()
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor(public oidcSecurityService: OidcSecurityService) {

      let openIDImplicitFlowConfiguration = new OpenIDImplicitFlowConfiguration();
      openIDImplicitFlowConfiguration.stsServer = 'https://localhost:44318';
      openIDImplicitFlowConfiguration.redirect_url = 'https://localhost:44311';
      openIDImplicitFlowConfiguration.client_id = 'angularclient';
      openIDImplicitFlowConfiguration.response_type = 'id_token token';
      openIDImplicitFlowConfiguration.scope = 'openid email profile';
      openIDImplicitFlowConfiguration.post_logout_redirect_uri = 'https://localhost:44311/Unauthorized';
      openIDImplicitFlowConfiguration.startup_route = '/home';
      openIDImplicitFlowConfiguration.forbidden_route = '/Forbidden';
      openIDImplicitFlowConfiguration.unauthorized_route = '/Unauthorized';
      openIDImplicitFlowConfiguration.auto_userinfo = true;
      openIDImplicitFlowConfiguration.log_console_warning_active = true;
      openIDImplicitFlowConfiguration.log_console_debug_active = false;
      openIDImplicitFlowConfiguration.max_id_token_iat_offset_allowed_in_seconds = 10;
      openIDImplicitFlowConfiguration.override_well_known_configuration = false;
      openIDImplicitFlowConfiguration.override_well_known_configuration_url = 'https://localhost:44386/wellknownconfiguration.json';
      // openIDImplicitFlowConfiguration.storage = localStorage;
      
      this.oidcSecurityService.setupModule(openIDImplicitFlowConfiguration);
  }
}
