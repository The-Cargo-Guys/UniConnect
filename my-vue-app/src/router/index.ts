import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router';
import { useAuth0 } from '@auth0/auth0-vue'; // Import Auth0 authentication
import { 
    TheCoursesPage, TheAddCoursePage, TheHomePage, TheProfilePage, 
    TheSocietiesPage, TheRegistryPage, TheCallbackPage, TheAddSocietyPage, 
    TheSocietiesDetailsPage 
} from '../views';

const routes: Array<RouteRecordRaw> = [
    { path: '/', component: TheHomePage },
    { path: '/callback', component: TheCallbackPage }, // Must be public for Auth0 login

    // Protected routes
    { path: '/profile', component: TheProfilePage, meta: { requiresAuth: true } },
    { path: '/courses', component: TheCoursesPage, meta: { requiresAuth: true } },
    { path: '/add-course', component: TheAddCoursePage, meta: { requiresAuth: true } },
    { path: '/societies', component: TheSocietiesPage, meta: { requiresAuth: true } },
    { path: '/register', component: TheRegistryPage, meta: { requiresAuth: true } },
    { path: '/add-society', component: TheAddSocietyPage, meta: { requiresAuth: true } },
    { path: '/societies/:id', name: "SocietiesDetails", component: TheSocietiesDetailsPage, meta: { requiresAuth: true } },

    // Redirect unknown routes to home
    { path: '/:pathMatch(.*)*', redirect: '/' },
];

const router = createRouter({
    history: createWebHistory(),
    routes
});

// Navigation Guard: Protects routes based on authentication
router.beforeEach(async (to, _, next) => {
    const { isAuthenticated, isLoading, loginWithRedirect } = useAuth0();

    // Wait for Auth0 to finish loading
    if (isLoading.value) {
        await new Promise((resolve) => setTimeout(resolve, 500));
    }

    // Check if the route requires authentication
    if (to.meta.requiresAuth && !isAuthenticated.value) {
        loginWithRedirect();
    } else {
        next();
    }
});

export default router;
