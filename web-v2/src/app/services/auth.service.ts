import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { OAuthService, AuthConfig } from 'angular-oauth2-oidc';
import { Observable, of, throwError } from 'rxjs';
import { environment } from '../../environments/environment';

interface SignupRequest {
  username: string;
  email: string;
  password: string;
}

interface SignupResponse {
  success: boolean;
  message: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(
    private oauthService: OAuthService,
    private http: HttpClient
  ) {}

  initAuth(): void {
    const authConfig: AuthConfig = environment.authConfig;
    this.oauthService.configure(authConfig);
    this.oauthService.loadDiscoveryDocument();
    this.oauthService.tryLoginImplicitFlow();
  }

  getAccessToken(): string {
    return this.oauthService.getAccessToken();
  }

  isLoggedIn(): boolean {
    return this.oauthService.hasValidAccessToken();
  }

  login(username: string, password: string): void {
    const params = {
      username,
      password
    };

    this.oauthService.initCodeFlow('', params);
  }

  async handleAuthCallback(): Promise<boolean> {
    await this.oauthService.tryLoginCodeFlow();

    if (this.oauthService.hasValidAccessToken()) {
      return true;
    }

    return false;
  }

  logout(): void {
    this.oauthService.logOut();
  }

  signup(username: string, email: string, password: string): Observable<SignupResponse> {
    // TODO: Replace with actual API endpoint when backend is ready
    const signupRequest: SignupRequest = {
      username,
      email,
      password
    };

    // For now, return a mock success response
    // In production, uncomment the line below and configure the proper endpoint
    // return this.http.post<SignupResponse>('https://localhost:7002/api/auth/signup', signupRequest);
    
    // Mock successful signup
    return of({
      success: true,
      message: 'Account created successfully'
    });
  }
}
