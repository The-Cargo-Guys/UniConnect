import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router';
import { TheCommunitiesPage, TheHomePage, TheProfilePage, TheSocietiesPage, TheRegistryPage, TheCallbackPage, TheAddSocietyPage, TheSocietiesDetailsPage  } from '../views';

const routes: Array<RouteRecordRaw> = [
    { path: '/', component: TheHomePage },
    { path: '/profile', component: TheProfilePage },
    { path: '/communities', component: TheCommunitiesPage },
    { path: '/societies', component: TheSocietiesPage },
    { path: '/register', component: TheRegistryPage },
    { path: '/callback', component: TheCallbackPage },
    { path: "/add-society", component: TheAddSocietyPage },
    { path: "/societies/:id", name: "SocietiesDetails", component: TheSocietiesDetailsPage },
    { path: '/:pathMatch(.*)*', redirect: '/' },
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