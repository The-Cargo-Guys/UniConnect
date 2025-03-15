// src/router/index.ts
import { createRouter, createWebHistory } from 'vue-router';
import { authState } from '@/stores/auth';
import HomePage from '@/views/HomePage.vue';
import AuthPage from '@/views/AuthPage.vue';

const routes = [
  { path: '/', component: HomePage, meta: { requiresAuth: true } },
  { path: '/auth', component: AuthPage },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

router.beforeEach((to, from, next) => {
  if (to.meta.requiresAuth && !authState.isAuthenticated) {
    next('/auth');
  } else {
    next();
  }
});

export default router;
