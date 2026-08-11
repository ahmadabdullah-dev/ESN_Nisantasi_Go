import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import agent from "../api/agent";
import type { AddPlanDto, PlanDto, UpdatePlanDto } from "../types/plan";
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
      const response = await agent.get<boolean>(
        `/plan/is-participated/${planId}`,
      );
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
      queryClient.invalidateQueries({
        queryKey: ["user-plans"],
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
      queryClient.invalidateQueries({
        queryKey: ["user-plans"],
      });
    },
  });
};

export function useUserPlans(userId: string, pagination: PaginationParams) {
  return useQuery({
    queryKey: ["user-plans", userId, pagination],
    queryFn: async () => {
      const response = await agent.get<PaginatedList<PlanDto>>(
        `/plan/user/${userId}`,
        { params: pagination },
      );
      return response.data;
    },
    enabled: !!userId,
    retry: false,
  });
}
export const useUpdatePlan = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async (creds: UpdatePlanDto) => {
      const response = await agent.put<string>("/Plan", creds);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["plans"] });
    },
  });
};