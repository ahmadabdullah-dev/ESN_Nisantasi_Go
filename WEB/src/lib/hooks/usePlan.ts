import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import agent from "../api/agent";
import type { AddPlanDto, PlanDto } from "../types/plan";
import type { PaginatedList, PaginationParams } from "../types/common";

export const usePlan = (pagination?: PaginationParams) => {
  const getPlansAsync = useQuery({
    queryKey: ["plans", pagination?.page, pagination?.pageSize],
    queryFn: async () =>
      await agent
        .get<PaginatedList<PlanDto>>("/Plan/paged", { params: pagination })
        .then((res) => res.data),
    enabled: !!pagination,
    staleTime: 5 * 60 * 1000,
  });

  const addPlanAsync = useMutation({
    mutationFn: async (creds: AddPlanDto) => {
      const response = await agent.post("/Plan/add", creds);
      return response.data;
    },
  });

  return {
    getPlansAsync,
    addPlanAsync,
  };
};

export function usePlanById(id: string) {
  return useQuery({
    queryKey: ["plans", id],
    queryFn: async () =>
      agent
        .get<PlanDto>("/plan", { params: { planId: id } })
        .then((res) => res.data),
    staleTime: 5 * 60 * 1000,
    enabled: !!id,
    retry: false,
  });
}

export function useIsPlanParticipated(planId: string) {
  return useQuery({
    queryKey: ["plans", planId, "participated"],

    queryFn: async () => {
      const response = await agent.get(`/plan/is-participated/${planId}`);
      return response.data;
    },
    enabled: !!planId,
    retry: false,
  });
}

export const useJoinPlan = (planId: string) => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async () => {
      await agent.post(`/Plan/join/${planId}`);
    },

    onMutate: async () => {
      await queryClient.cancelQueries({
        queryKey: ["plans", planId, "participated"],
      });

      queryClient.setQueryData(["plans", planId, "participated"], true);
    },

    onError: () => {
      queryClient.invalidateQueries({
        queryKey: ["plans", planId, "participated"],
      });
    },

    onSettled: () => {
      queryClient.invalidateQueries({
        queryKey: ["plans", planId, "participated"],
      });
    },
  });
};

export const useLeavePlan = (planId: string) => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async () => {
      await agent.post(`/Plan/leave/${planId}`);
    },

    onMutate: async () => {
      await queryClient.cancelQueries({
        queryKey: ["plans", planId, "participated"],
      });
      queryClient.setQueryData(["plans", planId, "participated"], false);
    },

    onError: () => {
      queryClient.invalidateQueries({
        queryKey: ["plans", planId, "participated"],
      });
    },

    onSettled: () => {
      queryClient.invalidateQueries({
        queryKey: ["plans", planId, "participated"],
      });
    },
  });
};

export function useUserPlans(userId: string, p: PaginationParams) {
  return useQuery({
    queryKey: ["user-plans", userId, p],
    queryFn: async () => {
      const response = await agent.get(`/plan/user/${userId}`, { params: p });
      return response.data;
    },
    enabled: !!userId,
    retry: false,
  });
}