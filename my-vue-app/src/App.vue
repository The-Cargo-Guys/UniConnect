<script setup lang="ts">
import { useRouter, useRoute } from "vue-router";
import { RouterView } from "vue-router";
import TheNavBar from "./components/TheNavBar.vue";
import TheUserNavBar from "./components/TheUserNavBar.vue";
import Auth from "./views/Auth.vue"; // ✅ Correct import
import { ref, onMounted, watch } from "vue";

const router = useRouter();
const route = useRoute();
const isLoggedIn = ref(false);

// 🚀 **Check authentication on app load BEFORE rendering**
onMounted(() => {
	const userId = localStorage.getItem("userId");

	if (userId) {
		console.log("✅ Found stored userId, setting isLoggedIn = true");
		isLoggedIn.value = true;
	} else {
		console.log("❌ No userId found, checking if on /auth");
		isLoggedIn.value = false;

		// **Only redirect to /auth if NOT already there**
		if (route.path !== "/auth") {
			router.push("/auth");
		}
	}
});

// 🔄 **Watch localStorage for login/logout changes**
watch(
	() => localStorage.getItem("userId"),
	(newUserId) => {
		if (newUserId) {
			console.log("🔄 userId updated, setting isLoggedIn = true");
			isLoggedIn.value = true;
			router.push("/");
		} else {
			console.log("❌ userId removed, redirecting to /auth");
			isLoggedIn.value = false;
			router.push("/auth");
		}
	}
);

// ✅ **Handle Logout - Clears storage**
const handleLogout = () => {
	localStorage.removeItem("userId");
	sessionStorage.clear();
	isLoggedIn.value = false;
	router.push("/auth");
};
</script>

<template>
	<v-app class="fade-in">
		<!-- ⏳ Show loading state -->
		<template v-if="false">
			<div class="loading-screen">Loading...</div>
		</template>

		<!-- 🔑 Show login/register page if user is NOT authenticated -->
		<template v-else-if="!isLoggedIn">
			<Auth /> <!-- ✅ Correct component name -->
		</template>

		<!-- 🏠 Show main content when the user IS authenticated -->
		<template v-else>
			<TheNavBar />
			<TheUserNavBar @logout="handleLogout"/>
			<v-avatar class="logo-wrapper">
				<v-img
					src="/UniConnect.svg"
					alt="Logo"
					contain
				></v-img>
			</v-avatar>
			<v-spacer></v-spacer>
			<v-main>
				<RouterView />
			</v-main>
		</template>
	</v-app>
</template>

<style scoped>
.loading-screen {
	display: flex;
	justify-content: center;
	align-items: center;
	height: 100vh;
	font-size: 20px;
	font-weight: bold;
}

.logo-wrapper {
	position: fixed;
	background-color: rgb(255, 255, 255);
	margin-top: 10px;	
	height: 110px;
	width: 110px;
}
</style>
