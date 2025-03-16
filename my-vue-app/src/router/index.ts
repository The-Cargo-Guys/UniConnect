import Auth from "../views/Auth.vue"; // ✅ Correct import
import UserDetails from "../views/UserDetails.vue"; // ✅ Import User Details Page
import ProfileDetails from "../views/ProfileDetails.vue"; // ✅ Import Profile Details Page
import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router';
import { 
    TheCoursesPage, 
    TheAddCoursePage, 
    TheHomePage, 
    TheProfilePage, 
    TheEditUserPage, 
    TheSocietiesPage, 
    TheCallbackPage, 
    TheAddSocietyPage, 
    TheSocietiesDetailsPage, 
    TheCoursesDetailsPage,
    TheConnectPage
} from '../views';

const routes: Array<RouteRecordRaw> = [
    { path: "/", component: TheHomePage },
    { path: "/auth", component: Auth }, // ✅ Using Auth.vue
    { path: "/profile", component: TheProfilePage, meta: { requiresAuth: true } },
    { path: "/edit-user", component: TheEditUserPage, meta: { requiresAuth: true } },
    { path: "/courses", component: TheCoursesPage, meta: { requiresAuth: true } },
    { path: "/connect", component: TheConnectPage, meta: { requiresAuth: true } },
    { path: "/add-course", component: TheAddCoursePage, meta: { requiresAuth: true } },
    { path: "/societies", component: TheSocietiesPage, meta: { requiresAuth: true } },
    { path: "/callback", component: TheCallbackPage },
    { path: "/add-society", component: TheAddSocietyPage, meta: { requiresAuth: true } },
    { path: "/societies/:id", name: "SocietiesDetails", component: TheSocietiesDetailsPage, meta: { requiresAuth: true } },
    { path: "/courses/:id", name: "CoursesDetails", component: TheCoursesDetailsPage, meta: { requiresAuth: true } },
    { path: "/user-details", component: UserDetails, meta: { requiresAuth: true } }, // ✅ User Details Page
    { path: "/Views/ProfileDetails/:userId", name: "ProfileDetails", component: ProfileDetails, meta: { requiresAuth: true } }, // ✅ Added Profile Details Page
    { path: '/:pathMatch(.*)*', redirect: '/' },
];

const router = createRouter({
    history: createWebHistory(),
    routes
});

// 🔒 Protect routes that require authentication
router.beforeEach((to, _, next) => {
    const isLoggedIn = !!localStorage.getItem("userId"); // Checks if userId exists

    if (to.meta.requiresAuth && !isLoggedIn) {
        console.log("❌ Not logged in, redirecting to /auth");
        next("/auth");
    } else {
        next();
    }
});

export default router;
