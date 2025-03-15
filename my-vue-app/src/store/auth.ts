// src/stores/auth.ts
import { reactive } from 'vue';

export const authState = reactive({
  isAuthenticated: false,
  token: '',
});

export function setAuthentication(status: boolean, token: string = '') {
  authState.isAuthenticated = status;
  authState.token = token;
  if (status) {
    localStorage.setItem('token', token);
  } else {
    localStorage.removeItem('token');
  }
}
