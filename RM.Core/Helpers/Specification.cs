// ***********************************************************************
// Assembly         : Technosignage.Utilities
// Author           : Technosignage
// Created          : 01-02-2019
//
// Last Modified By : Technosignage
// Last Modified On : 02-03-2019
// ***********************************************************************
// <copyright file="Specification.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace RM.Core.Helpers
{
    /// <summary>
    /// Class Specification.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Specification<T>
    {
        /// <summary>
        /// All
        /// </summary>
        public static readonly Specification<T> All = new IdentitySpecification<T>();

        /// <summary>
        /// Determines whether [is satisfied by] [the specified entity].
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns><c>true</c> if [is satisfied by] [the specified entity]; otherwise, <c>false</c>.</returns>
        public bool IsSatisfiedBy(T entity)
        {
            Func<T, bool> predicate = ToExpression().Compile();
            return predicate(entity);
        }

        /// <summary>
        /// Converts to expression.
        /// </summary>
        /// <returns>Expression&lt;Func&lt;T, System.Boolean&gt;&gt;.</returns>
        public abstract Expression<Func<T, bool>> ToExpression();

        /// <summary>
        /// Ands the specified specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>Specification&lt;T&gt;.</returns>
        public Specification<T> And(Specification<T> specification)
        {
            if (this == All)
                return specification;
            if (specification == All)
                return this;

            return new AndSpecification<T>(this, specification);
        }

        /// <summary>
        /// Ors the specified specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>Specification&lt;T&gt;.</returns>
        public Specification<T> Or(Specification<T> specification)
        {
            if (this == All || specification == All)
                return All;

            return new OrSpecification<T>(this, specification);
        }

        /// <summary>
        /// Nots this instance.
        /// </summary>
        /// <returns>Specification&lt;T&gt;.</returns>
        public Specification<T> Not()
        {
            return new NotSpecification<T>(this);
        }
    }

    /// <summary>
    /// Class ReplaceExpressionVisitor.
    /// Implements the <see cref="ExpressionVisitor" />
    /// </summary>
    /// <seealso cref="ExpressionVisitor" />
    internal class ReplaceExpressionVisitor : ExpressionVisitor
    {
        /// <summary>
        /// The old value
        /// </summary>
        private readonly Expression _oldValue;
        /// <summary>
        /// The new value
        /// </summary>
        private readonly Expression _newValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplaceExpressionVisitor"/> class.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }

        /// <summary>
        /// Dispatches the expression to one of the more specialized visit methods in this class.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        public override Expression Visit(Expression node)
        {
            if (node == _oldValue)
                return _newValue;
            return base.Visit(node);
        }
    }

    /// <summary>
    /// Class ParameterReplacer.
    /// Implements the <see cref="ExpressionVisitor" />
    /// </summary>
    /// <seealso cref="ExpressionVisitor" />
    internal class ParameterReplacer : ExpressionVisitor
    {
        /// <summary>
        /// The parameter
        /// </summary>
        private readonly ParameterExpression _parameter;

        /// <summary>
        /// Visits the <see cref="T:System.Linq.Expressions.ParameterExpression"></see>.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return base.VisitParameter(_parameter);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterReplacer"/> class.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        internal ParameterReplacer(ParameterExpression parameter)
        {
            _parameter = parameter;
        }
    }

    /// <summary>
    /// Class IdentitySpecification. This class cannot be inherited.
    /// Implements the <see cref="Technosignage.Utilities.Helpers.Specification{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Technosignage.Utilities.Helpers.Specification{T}" />
    internal sealed class IdentitySpecification<T> : Specification<T>
    {
        /// <summary>
        /// Converts to expression.
        /// </summary>
        /// <returns>Expression&lt;Func&lt;T, System.Boolean&gt;&gt;.</returns>
        public override Expression<Func<T, bool>> ToExpression()
        {
            return x => true;
        }
    }

    /// <summary>
    /// Class AndSpecification. This class cannot be inherited.
    /// Implements the <see cref="Technosignage.Utilities.Helpers.Specification{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Technosignage.Utilities.Helpers.Specification{T}" />
    internal sealed class AndSpecification<T> : Specification<T>
    {
        /// <summary>
        /// The left
        /// </summary>
        private readonly Specification<T> _left;
        /// <summary>
        /// The right
        /// </summary>
        private readonly Specification<T> _right;

        /// <summary>
        /// Initializes a new instance of the <see cref="AndSpecification{T}"/> class.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        public AndSpecification(Specification<T> left, Specification<T> right)
        {
            _right = right;
            _left = left;
        }

        /// <summary>
        /// Converts to expression.
        /// </summary>
        /// <returns>Expression&lt;Func&lt;T, System.Boolean&gt;&gt;.</returns>
        public override Expression<Func<T, bool>> ToExpression()
        {
            Expression<Func<T, bool>> leftExpression = _left.ToExpression();
            Expression<Func<T, bool>> rightExpression = _right.ToExpression();


            //BinaryExpression andExpression = Expression.AndAlso(leftExpression.Body, rightExpression.Body);
            //return Expression.Lambda<Func<T, bool>>(andExpression, leftExpression.Parameters.Single());


            var paramExpr = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(leftExpression.Parameters[0], paramExpr);
            var left = leftVisitor.Visit(leftExpression.Body);

            var rightVisitor = new ReplaceExpressionVisitor(rightExpression.Parameters[0], paramExpr);
            var right = rightVisitor.Visit(rightExpression.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(left, right), paramExpr);
        }
    }


    /// <summary>
    /// Class OrSpecification. This class cannot be inherited.
    /// Implements the <see cref="Technosignage.Utilities.Helpers.Specification{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Technosignage.Utilities.Helpers.Specification{T}" />
    internal sealed class OrSpecification<T> : Specification<T>
    {
        /// <summary>
        /// The left
        /// </summary>
        private readonly Specification<T> _left;
        /// <summary>
        /// The right
        /// </summary>
        private readonly Specification<T> _right;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrSpecification{T}"/> class.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        public OrSpecification(Specification<T> left, Specification<T> right)
        {
            _right = right;
            _left = left;
        }

        /// <summary>
        /// Converts to expression.
        /// </summary>
        /// <returns>Expression&lt;Func&lt;T, System.Boolean&gt;&gt;.</returns>
        public override Expression<Func<T, bool>> ToExpression()
        {
            Expression<Func<T, bool>> leftExpression = _left.ToExpression();
            Expression<Func<T, bool>> rightExpression = _right.ToExpression();

            //BinaryExpression orExpression = Expression.OrElse(leftExpression.Body, rightExpression.Body);

            //return Expression.Lambda<Func<T, bool>>(orExpression, leftExpression.Parameters.Single());

            var paramExpr = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(leftExpression.Parameters[0], paramExpr);
            var left = leftVisitor.Visit(leftExpression.Body);

            var rightVisitor = new ReplaceExpressionVisitor(rightExpression.Parameters[0], paramExpr);
            var right = rightVisitor.Visit(rightExpression.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.OrElse(left, right), paramExpr);
        }
    }


    /// <summary>
    /// Class NotSpecification. This class cannot be inherited.
    /// Implements the <see cref="Technosignage.Utilities.Helpers.Specification{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Technosignage.Utilities.Helpers.Specification{T}" />
    internal sealed class NotSpecification<T> : Specification<T>
    {
        /// <summary>
        /// The specification
        /// </summary>
        private readonly Specification<T> _specification;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotSpecification{T}"/> class.
        /// </summary>
        /// <param name="specification">The specification.</param>
        public NotSpecification(Specification<T> specification)
        {
            _specification = specification;
        }

        /// <summary>
        /// Converts to expression.
        /// </summary>
        /// <returns>Expression&lt;Func&lt;T, System.Boolean&gt;&gt;.</returns>
        public override Expression<Func<T, bool>> ToExpression()
        {
            Expression<Func<T, bool>> expression = _specification.ToExpression();
            //UnaryExpression notExpression = Expression.Not(expression.Body);

            //return Expression.Lambda<Func<T, bool>>(notExpression, expression.Parameters.Single());

            var paramExpr = Expression.Parameter(typeof(T));

            var rightVisitor = new ReplaceExpressionVisitor(expression.Parameters[0], paramExpr);
            var right = rightVisitor.Visit(expression.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.Not(expression), paramExpr);
        }
    }
}
