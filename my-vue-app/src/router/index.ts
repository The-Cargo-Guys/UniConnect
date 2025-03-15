import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router';
import { TheCoursesPage, TheHomePage, TheProfilePage, TheSocietiesPage, TheRegistryPage, TheCallbackPage, TheAddSocietyPage, TheSocietiesDetailsPage } from '../views';
import Login from '../views/Login.vue';
import Register from '../views/Register.vue';
import Dashboard from '../views/Dashboard.vue';

const routes: Array<RouteRecordRaw> = [
    { path: '/', component: TheHomePage },
    { path: '/profile', component: TheProfilePage, meta: { requiresAuth: true } },
    { path: '/courses', component: TheCoursesPage },
    { path: '/societies', component: TheSocietiesPage },
    { path: '/register', component: Register },
    { path: '/callback', component: TheCallbackPage },
    { path: '/login', component: Login },
    { path: '/dashboard', component: Dashboard, meta: { requiresAuth: true } },
    { path: '/add-society', component: TheAddSocietyPage, meta: { requiresAuth: true } },
    { path: '/societies/:id', name: 'SocietiesDetails', component: TheSocietiesDetailsPage },
    { path: '/:pathMatch(.*)*', redirect: '/' },
];

const router = createRouter({
    history: createWebHistory(),
    routes
});

router.beforeEach((to, _, next) => {
    const token = localStorage.getItem('token');
    const isAuthenticated = !!token;

    if (to.meta.requiresAuth && !isAuthenticated) {
        next('/login'); // Redirect unauthorized users
    } else {
        next();
    }
});

export default router;
