import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router';
import { TheCommunitiesPage, TheHomePage, TheSocietiesPage, TheRegistryPage, TheCallbackPage } from '../views';

const routes: Array<RouteRecordRaw> = [
    { path: '/', component: TheHomePage },
    { path: '/communities', component: TheCommunitiesPage },
    { path: '/societies', component: TheSocietiesPage },
    { path: '/register', component: TheRegistryPage },
    { path: '/:pathMatch(.*)*', redirect: '/' },
    { path: '/callback', component: TheCallbackPage }
];

const router = createRouter({
    history: createWebHistory(),
    routes
});

router.beforeEach((to, _, next) => {
    // const isLoggedIn = localStorage.getItem('LoggedIn') === 'true';
    // if (to.matched.some((record) => record.meta.auth) && !isLoggedIn) {
    //     next('/login');
    // } else {
    //     next();
    // }
    next();
});

export default router;
